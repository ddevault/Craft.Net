using System;
using Craft.Net.Common;
using Craft.Net.Anvil;
using System.Collections.Generic;
using System.Reflection;
using Craft.Net.Physics;

namespace Craft.Net.Logic
{
    public abstract class Block : Item
    {
        public override short ItemId { get { return BlockId; } }
        
        public delegate BoundingBox? BoundingBoxHandler(BlockInfo info);
        public delegate bool IsSolidOnFaceHandler(BlockInfo info, BlockFace face);
        public delegate void BlockMinedHandler(World world, Coordinates3D coordinates, BlockInfo info);
        public delegate bool BlockRightClickedHandler(World world, Coordinates3D coordinates, BlockInfo info, BlockFace face, Coordinates3D cursor, ItemInfo? item);
        
        private static Dictionary<short, string> BlockNames { get; set; }
        private static Dictionary<short, double> BlockHardness { get; set; }
        public static IBlockPhysicsProvider PhysicsProvider { get; private set; }
        private static Dictionary<short, string> BlockPlacementSoundEffects { get; set; }
        private static Dictionary<short, BoundingBoxHandler> BoundingBoxHandlers { get; set; }
        private static Dictionary<short, IsSolidOnFaceHandler> IsSolidOnFaceHandlers { get; set; }
        private static Dictionary<short, BlockMinedHandler> BlockMinedHandlers { get; set; }
        private static Dictionary<short, BlockRightClickedHandler> BlockRightClickedHandlers { get; set; }
        
        static Block()
        {
            PhysicsProvider = new BlockPhysicsProvider();
            BoundingBoxHandlers = new Dictionary<short, BoundingBoxHandler>();
            IsSolidOnFaceHandlers = new Dictionary<short, IsSolidOnFaceHandler>();
            BlockMinedHandlers = new Dictionary<short, BlockMinedHandler>();
            BlockRightClickedHandlers = new Dictionary<short, BlockRightClickedHandler>();
            BlockHardness = new Dictionary<short, double>();
            BlockNames = new Dictionary<short, string>();
            BlockPlacementSoundEffects = new Dictionary<short, string>();
            ReflectBlocks(typeof(Block).Assembly);
        }

        public static void ReflectBlocks(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(Block).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    LoadBlock((Block)Activator.CreateInstance(type));
                }
            }
        }

        public static void LoadBlock<T>() where T : Block
        {
            LoadBlock(default(T));
        }
        
        private static void LoadBlock(Block block)
        {
            BlockNames.Add(block.BlockId, block.Name);
            Item.LoadItem(block);
            if (!Item.ItemUsedOnBlockHandlers.ContainsKey(block.BlockId))
                Item.ItemUsedOnBlockHandlers[block.BlockId] = DefaultUsedOnBlockHandler;
        }
        
        public static string GetPlacementSoundEffect(short blockId)
        {
            if (BlockPlacementSoundEffects.ContainsKey(blockId))
                return BlockPlacementSoundEffects[blockId];
            return SoundEffect.DigStone;
        }
        
        protected void SetPlacementSoundEffect(string soundEffect)
        {
            BlockPlacementSoundEffects[BlockId] = soundEffect;
        }
        
        protected void SetBoundingBoxHandler(BoundingBoxHandler handler)
        {
            BoundingBoxHandlers[BlockId] = handler;
        }
        
        protected void SetIsSolidOnFaceHandler(IsSolidOnFaceHandler handler)
        {
            IsSolidOnFaceHandlers[BlockId] = handler;
        }

        protected void SetBlockMinedHandler(BlockMinedHandler handler)
        {
            BlockMinedHandlers[BlockId] = handler;
        }
        
        protected void SetBlockRightClickedHandler(BlockRightClickedHandler handler)
        {
            BlockRightClickedHandlers[BlockId] = handler;
        }

        protected void SetDropHandler(Func<World, Coordinates3D, BlockInfo, ItemStack[]> dropHandler, bool overrideSilkTouch = false)
        {
            SetBlockMinedHandler((world, coordinates, info) =>
                {
                    world.SetBlockId(coordinates, 0);
                    world.SetMetadata(coordinates, 0);
                    foreach (var drop in dropHandler(world, coordinates, info))
                        world.OnSpawnEntityRequested(new ItemEntity((Vector3)coordinates + new Vector3(0.5), drop));
                });
        }

        private class BlockPhysicsProvider : IBlockPhysicsProvider
        {
            public BoundingBox? GetBoundingBox(World world, Coordinates3D coordinates)
            {
                // TODO: Consider passing block info to a handler to get a fancier bounding box
                var info = world.GetBlockInfo(coordinates);
                return Block.GetBoundingBox(info);
            }
        }
        
        public static BoundingBox? GetBoundingBox(BlockInfo info)
        {
            if (BoundingBoxHandlers.ContainsKey(info.BlockId))
                return BoundingBoxHandlers[info.BlockId](info);
            else
                return DefaultBoundingBoxHandler(info); 
        }
        
        public static bool GetIsSolidOnFace(BlockInfo info, BlockFace face)
        {
            if (IsSolidOnFaceHandlers.ContainsKey(info.BlockId))
                return IsSolidOnFaceHandlers[info.BlockId](info, face);
            else
                return DefaultIsSolidOnFaceHandler(info, face);
        }

        internal static void OnBlockMined(World world, Coordinates3D coordinates)
        {
            var info = world.GetBlockInfo(coordinates);
            if (BlockMinedHandlers.ContainsKey(info.BlockId))
                BlockMinedHandlers[info.BlockId](world, coordinates, info);
            else
                DefaultBlockMinedHandler(world, coordinates, info);
        }
        
        internal static bool OnBlockRightClicked(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo? item)
        {
            var info = world.GetBlockInfo(coordinates);
            if (BlockRightClickedHandlers.ContainsKey(info.BlockId))
                return BlockRightClickedHandlers[info.BlockId](world, coordinates, info, face, cursor, item);
            else
                return DefaultBlockRightClickedHandler(world, coordinates, info, face, item);
        }
        
        public static int GetHarvestTime(short blockId, short itemId, World world, PlayerEntity entity, out short damage)
        {
            int time = GetHarvestTime(blockId, itemId, out damage);
            var type = Item.GetToolType(itemId);
            if (type != null)
            {
                if (!Item.IsEfficient(itemId, blockId))
                {
                    //if (entity.IsUnderwater(world) && !entity.IsOnGround(world))
                    //    time *= 25;
                    //else if (entity.IsUnderwater(world) || !entity.IsOnGround(world))
                    //   time *= 5;
                    // TODO
                }
            }
            return time;
        }
        
        public static double GetBlockHardness(short blockId)
        {
            if (BlockHardness.ContainsKey(blockId))
                return BlockHardness[blockId];
            return 0;
        }

        /// <summary>
        /// Gets the amount of time (in milliseconds) it takes to harvest this
        /// block with the given tool.
        /// </summary>
        public static int GetHarvestTime(short blockId, short itemId, out short damage)
        {
            // time is in seconds until returned
            double hardness = GetBlockHardness(blockId);
            if (hardness == -1)
            {
                damage = 0;
                return -1;
            }
            double time = hardness * 1.5;
            var tool = Item.GetToolType(itemId);
            var material = Item.GetItemMaterial(itemId);
            damage = 0;
            // Adjust for tool in use
            if (tool != null)
            {
                //if (!CanHarvest(tool))
                //    time *= 3.33;
                if (Item.IsEfficient(itemId, blockId) && material != null)
                {
                    switch (material.Value)
                    {
                        case ItemMaterial.Wood:
                            time /= 2;
                            break;
                        case ItemMaterial.Stone:
                            time /= 4;
                            break;
                        case ItemMaterial.Iron:
                            time /= 6;
                            break;
                        case ItemMaterial.Diamond:
                            time /= 8;
                            break;
                        case ItemMaterial.Gold:
                            time /= 12;
                            break;
                    }
                }
                // Do tool damage
                damage = 1;
                if (tool.Value == Craft.Net.Logic.ToolType.Pickaxe
                    || tool.Value == Craft.Net.Logic.ToolType.Axe
                    || tool.Value == Craft.Net.Logic.ToolType.Shovel)
                {
                    damage = (short)(hardness != 0 ? 1 : 0);
                }
                else if (tool.Value == Craft.Net.Logic.ToolType.Sword)
                {
                    damage = (short)(hardness != 0 ? 2 : 0);
                    time /= 1.5;
                    //if (this is CobwebBlock) // TODO
                    //    time /= 15;
                }
                else if (tool.Value == Craft.Net.Logic.ToolType.Hoe)
                    damage = 0;
//               else if (tool is ShearsItem) // TODO
//                {
//                    if (this is WoolBlock)
//                        time /= 5;
//                    if (this is LeavesBlock || this is CobwebBlock)
//                        time /= 15;
//                    if (this is CobwebBlock || this is LeavesBlock || this is TallGrassBlock ||
//                        this is TripwireBlock || this is VineBlock)
//                        damage = 1;
//                    else
//                        damage = 0;
//                }
                else
                    damage = 0;
            }
            return (int)(time * 1000);
        }

        private static void DefaultBlockMinedHandler(World world, Coordinates3D coordinates, BlockInfo info)
        {
            world.SetBlockId(coordinates, 0);
            world.SetMetadata(coordinates, 0);
            world.OnSpawnEntityRequested(new ItemEntity((Vector3)coordinates + new Vector3(0.5),
                new ItemStack(info.BlockId, 1, info.Metadata)));
        }
        
        /*private */internal static void DefaultUsedOnBlockHandler(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item)
        {
            coordinates += MathHelper.BlockFaceToCoordinates(face);
            world.SetBlockId(coordinates, item.ItemId);
            world.SetMetadata(coordinates, (byte)item.Metadata);
        }
        
        private static bool DefaultBlockRightClickedHandler(World world, Coordinates3D coordinates, BlockInfo block, BlockFace face, ItemInfo? item)
        {
            return true;
        }
        
        private static BoundingBox? DefaultBoundingBoxHandler(BlockInfo info)
        {
            return new BoundingBox(Vector3.Zero, Vector3.One);
        }
        
        private static bool DefaultIsSolidOnFaceHandler(BlockInfo info, BlockFace face)
        {
            return true;   
        }
        
        public abstract short BlockId { get; }

        protected Block(string name, ItemMaterial? material = null, ToolType? toolType = null, double? hardness = null)
            : base(name, material, toolType)
        {
            if (hardness != null)
                BlockHardness[BlockId] = hardness.Value;
        }
    }
}