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
        
        private static Dictionary<short, BoundingBox?> BoundingBoxes { get; set; }
        private static Dictionary<short, string> BlockNames { get; set; }
        public static IBlockPhysicsProvider PhysicsProvider { get; private set; }
        public delegate void BlockMinedHandler(World world, Coordinates3D coordinates, BlockInfo info);
        private static Dictionary<short, BlockMinedHandler> BlockMinedHandlers { get; set; }
        public delegate bool BlockRightClickedHandler(World world, Coordinates3D coordinates, BlockInfo info, BlockFace face, Coordinates3D cursor, ItemInfo? item);
        private static Dictionary<short, BlockRightClickedHandler> BlockRightClickedHandlers { get; set; }
        private static Dictionary<short, string> BlockPlacementSoundEffects { get; set; }
        
        static Block()
        {
            PhysicsProvider = new BlockPhysicsProvider();
            BoundingBoxes = new Dictionary<short, BoundingBox?>();
            BlockMinedHandlers = new Dictionary<short, BlockMinedHandler>();
            BlockRightClickedHandlers = new Dictionary<short, BlockRightClickedHandler>();
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

        protected void SetBoundingBox(BoundingBox? boundingBox)
        {
            BoundingBoxes[BlockId] = boundingBox;
        }

        public static BoundingBox? GetBoundingBox(short blockId)
        {
            if (BoundingBoxes.ContainsKey(blockId))
                return BoundingBoxes[blockId];
            return new BoundingBox(Vector3.Zero, Vector3.One);
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

        protected void SetBlockMinedHandler(BlockMinedHandler handler)
        {
            BlockMinedHandlers[BlockId] = handler;
        }
        
        protected void SetBlockRightClickedHandler(BlockRightClickedHandler handler)
        {
            BlockRightClickedHandlers[BlockId] = handler;
        }

        private class BlockPhysicsProvider : IBlockPhysicsProvider
        {
            public BoundingBox? GetBoundingBox(World world, Coordinates3D coordinates)
            {
                // TODO: Consider passing block info to a handler to get a fancier bounding box
                var blockId = world.GetBlockId(coordinates);
                return Block.GetBoundingBox(blockId);
            }
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
        
        public abstract short BlockId { get; }

        protected Block(string name, ItemMaterial? material = null, ToolType? toolType = null)
            : base(name, material, toolType)
        {
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
    }
}