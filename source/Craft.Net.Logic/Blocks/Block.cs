using Craft.Net.Anvil;
using Craft.Net.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public struct BlockDescriptor
    {
        public short Id;
        public byte Metadata;
        public byte BlockLight;
        public byte SkyLight;

        public BlockDescriptor(short id)
        {
            Id = id;
            Metadata = BlockLight = SkyLight = 0;
        }

        public BlockDescriptor(short id, byte metadata)
        {
            Id = id;
            Metadata = metadata;
            BlockLight = SkyLight = 0;
        }

        public BlockDescriptor(short id, byte metadata, byte blockLight, byte skyLight)
        {
            Id = id;
            Metadata = metadata;
            BlockLight = blockLight;
            SkyLight = skyLight;
        }
    }

    public struct BlockLogicDescriptor
    {
        public BlockLogicDescriptor(Type blockType)
        {
            BlockType = blockType;

            BlockRightClicked = Block.DefaultBlockRightClickedHandler;
            BlockPlaced = Block.DefaultBlockPlacedHandler;
            BlockMined = Block.DefaultBlockMinedHandler;
            CanHarvest = Block.DefaultCanHarvestHandler;
			GetSupportDirection = Block.DefaultGetSupportDirectionHandler;
			BlockUpdated = Block.DefaultBlockUpdateHandler;
			GetDrop = Block.DefaultGetDropHandler;

            Hardness = 0;
            DisplayName = null;
            BoundingBox = new BoundingBox(Vector3.Zero, Vector3.One);
        }

        public Type BlockType;

        public Block.BlockRightClickedDelegate BlockRightClicked;
        public Block.BlockPlacedDelegate BlockPlaced;
        public Block.BlockMinedDelegate BlockMined;
        public Block.CanHarvestDelegate CanHarvest;
		public Block.GetSupportDirectionDelegate GetSupportDirection;
		public Block.BlockUpdateDelegate BlockUpdated;
		public Block.GetDropDelegate GetDrop;

        public double Hardness;
        public string DisplayName;
        public BoundingBox? BoundingBox;
    }

    public static class Block
    {
        public delegate BlockLogicDescriptor BlockLogicInitializer(BlockLogicDescriptor descriptor);

        public delegate bool BlockRightClickedDelegate(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition);
        public delegate void BlockPlacedDelegate(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition);
        public delegate void BlockMinedDelegate(BlockDescriptor block, World world, Coordinates3D destroyedBlock, ItemDescriptor? tool);
        public delegate bool CanHarvestDelegate(ItemDescriptor item, BlockDescriptor block);
		public delegate SupportDirection GetSupportDirectionDelegate(BlockDescriptor block, World world, Coordinates3D coordinates);
		public delegate void BlockUpdateDelegate(BlockDescriptor block, World world, Coordinates3D updatedBlock);
		public delegate ItemStack[] GetDropDelegate(BlockDescriptor block, World world, Coordinates3D minedBlock);

        public static BlockMinedDelegate GlobalDefaultBlockMinedHandler { get; set; }

        private static Dictionary<short, BlockLogicDescriptor> BlockLogicDescriptors { get; set; }

        static Block()
        {
            LoadBlocks();
        }

        private static void LoadBlocks()
        {
            if (BlockLogicDescriptors != null)
                return;
            BlockLogicDescriptors = new Dictionary<short, BlockLogicDescriptor>();
            // Loads all block classes in Craft.Net.Logic
            var types = typeof(Block).Assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(BlockAttribute), true).Any()).ToArray();
            LoadTypes(types);
        }

        public static void LoadTypes(Type[] types)
        {
            foreach (var type in types)
            {
                var attribute = (BlockAttribute)type.GetCustomAttributes(typeof(BlockAttribute), false).First();
                var method = type.GetMethods().FirstOrDefault(m => m.Name == attribute.Initializer && m.ReturnType == typeof(BlockLogicDescriptor)
                    && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(BlockLogicDescriptor) && !m.IsGenericMethod);
                var descriptor = new BlockLogicDescriptor(type);
                descriptor.Hardness = attribute.Hardness;
                descriptor.DisplayName = attribute.DisplayName;
                if (method != null)
                    descriptor = (BlockLogicDescriptor)method.Invoke(null, new object[] { descriptor });
                BlockLogicDescriptors[attribute.BlockId] = descriptor;
            }
        }

        public static ItemLogicDescriptor Initialize(ItemLogicDescriptor descriptor)
        {
            descriptor.ItemUsedOnBlock = OnItemUsedOnBlock;
            return descriptor;
        }

        public static void OnItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition)
        {
            var clicked = world.GetBlock(clickedBlock);
            if (OnBlockRightClicked(clicked, world, clickedBlock, clickedSide, cursorPosition))
            {
                if ((clickedBlock + clickedSide).Y >= 0 && (clickedBlock + clickedSide).Y <= Chunk.Height)
                    OnBlockPlaced(new BlockDescriptor(item.Id, (byte)item.Metadata), world, clickedBlock, clickedSide, cursorPosition);
            }
        }

        public static bool OnBlockRightClicked(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition)
        {
            if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
            return BlockLogicDescriptors[block.Id].BlockRightClicked(block, world, clickedBlock, clickedSide, cursorPosition);
        }

        public static void OnBlockPlaced(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition)
        {
            if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
            BlockLogicDescriptors[block.Id].BlockPlaced(block, world, clickedBlock, clickedSide, cursorPosition);
        }

        public static void OnBlockMined(BlockDescriptor block, World world, Coordinates3D destroyedBlock, ItemDescriptor? tool)
        {
            if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
            BlockLogicDescriptors[block.Id].BlockMined(block, world, destroyedBlock, tool);
        }

		public static void OnBlockUpdate(BlockDescriptor block, World world, Coordinates3D updatedBlock)
		{
			if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
			BlockLogicDescriptors[block.Id].BlockUpdated(block, world, updatedBlock);
		}

		public static ItemStack[] GetBlockDrop (BlockDescriptor block, World world, Coordinates3D minedBlock)
		{
			if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
			return BlockLogicDescriptors[block.Id].GetDrop(block, world, minedBlock);
		}

        public static bool CanHarvest(ItemDescriptor item, BlockDescriptor block)
        {
            return GetLogicDescriptor(block).CanHarvest(item, block);
        }

        public static BoundingBox? GetBoundingBox(short blockId)
        {
            return GetLogicDescriptor(blockId).BoundingBox;
        }

        /// <summary>
        /// Gets the amount of time (in milliseconds) it takes to harvest a block with the specified tool.
        /// </summary>
        public static int GetHarvestTime(ItemDescriptor item, BlockDescriptor block, out short damage)
        {
            var logic = GetLogicDescriptor(block);
            // time is in seconds until returned
            if (logic.Hardness == -1)
            {
                damage = 0;
                return -1;
            }
            double time = logic.Hardness * 1.5;
            damage = 0;
            // TODO: ToolItem class
            // Adjust for tool in use
            //if (tool != null)
            //{
            //    if (!CanHarvest(item, block))
            //        time *= 3.33;
            //    if (Tool.IsEfficient(item, block))
            //    {
            //        switch (tool.ToolMaterial)
            //        {
            //            case ToolMaterial.Wood:
            //                time /= 2;
            //                break;
            //            case ToolMaterial.Stone:
            //                time /= 4;
            //                break;
            //            case ToolMaterial.Iron:
            //                time /= 6;
            //                break;
            //            case ToolMaterial.Diamond:
            //                time /= 8;
            //                break;
            //            case ToolMaterial.Gold:
            //                time /= 12;
            //                break;
            //        }
            //    }
            //    // Do tool damage
            //    damage = 1;
            //    if (tool.ToolType == ToolType.Pick || tool.ToolType == ToolType.Axe || tool.ToolType == ToolType.Shovel)
            //        damage = (short)(Hardness != 0 ? 1 : 0);
            //    else if (tool.ToolType == ToolType.Sword)
            //    {
            //        damage = (short)(Hardness != 0 ? 2 : 0);
            //        time /= 1.5;
            //        if (block.Id == CobwebBlock.BlockId)
            //            time /= 15;
            //    }
            //    else if (tool.ToolType == ToolType.Hoe)
            //        damage = 0;
            //    else if (tool is ShearsItem)
            //    {
            //        if (this is WoolBlock)
            //            time /= 5;
            //        if (this is LeavesBlock || this is CobwebBlock)
            //            time /= 15;
            //        if (this is CobwebBlock || this is LeavesBlock || this is TallGrassBlock ||
            //            this is TripwireBlock || this is VineBlock)
            //            damage = 1;
            //        else
            //            damage = 0;
            //    }
            //    else
            //        damage = 0;
            //}
            return (int)(time * 1000);
        }

		public static void DoBlockUpdates(World world, Coordinates3D coordinates)
		{
			OnBlockUpdate(world.GetBlock(coordinates), world, coordinates);

            if ((coordinates + Coordinates3D.Up).Y < Chunk.Height)
                OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.Up), world, coordinates + Coordinates3D.Up);
            if ((coordinates + Coordinates3D.Down).Y >= 0)
                OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.Down), world, coordinates + Coordinates3D.Down);

			OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.North), world, coordinates + Coordinates3D.North);
            OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.South), world, coordinates + Coordinates3D.South);
            OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.East), world, coordinates + Coordinates3D.East);
            OnBlockUpdate(world.GetBlock(coordinates + Coordinates3D.West), world, coordinates + Coordinates3D.West);
		}

        public static BlockLogicDescriptor GetLogicDescriptor(BlockDescriptor block)
        {
            if (!BlockLogicDescriptors.ContainsKey(block.Id))
                throw new KeyNotFoundException("The given block does not exist.");
            return BlockLogicDescriptors[block.Id];
        }

        public static BlockLogicDescriptor GetLogicDescriptor(short block)
        {
            if (!BlockLogicDescriptors.ContainsKey(block))
                throw new KeyNotFoundException("The given block does not exist.");
            return BlockLogicDescriptors[block];
        }

        #region Default Handlers

        internal static bool DefaultBlockRightClickedHandler(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition)
        {
            return true;
        }

        internal static void DefaultBlockPlacedHandler(BlockDescriptor block, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates3D cursorPosition)
        {
            if (world.GetBlockId(clickedBlock + clickedSide) == 0) // TODO: There are more situations than just air when a block can be overwritten
            {
                world.SetBlockId(clickedBlock + clickedSide, block.Id);
                world.SetMetadata(clickedBlock + clickedSide, block.Metadata);
            }
        }

        internal static void DefaultBlockMinedHandler(BlockDescriptor block, World world, Coordinates3D destroyedBlock, ItemDescriptor? tool)
        {
            if (GlobalDefaultBlockMinedHandler == null)
            {
                world.SetBlockId(destroyedBlock, 0);
                world.SetMetadata(destroyedBlock, 0);
            }
            else
                GlobalDefaultBlockMinedHandler(block, world, destroyedBlock, tool);
        }

        internal static bool DefaultCanHarvestHandler(ItemDescriptor item, BlockDescriptor block)
        {
            if (GetLogicDescriptor(block).Hardness == -1)
                return false;
            return true;
        }

		internal static SupportDirection DefaultGetSupportDirectionHandler(BlockDescriptor block, World world, Coordinates3D coordinates)
		{
			return SupportDirection.None;
		}

		internal static void DefaultBlockUpdateHandler(BlockDescriptor block, World world, Coordinates3D updatedBlock)
		{
			var logic = GetLogicDescriptor(block);
			var support = logic.GetSupportDirection(block, world, updatedBlock);
			var supportingBlock = updatedBlock;
			switch (support)
			{
				case SupportDirection.Down:
					supportingBlock = updatedBlock + Coordinates3D.Down;
					break;
				case SupportDirection.Up:
					supportingBlock = updatedBlock + Coordinates3D.Up;
					break;
				case SupportDirection.East:
					supportingBlock = updatedBlock + Coordinates3D.East;
					break;
				case SupportDirection.West:
					supportingBlock = updatedBlock + Coordinates3D.West;
					break;
				case SupportDirection.North:
					supportingBlock = updatedBlock + Coordinates3D.North;
					break;
				case SupportDirection.South:
					supportingBlock = updatedBlock + Coordinates3D.South;
					break;
				default:
					return;
			}
			if (!World.IsValidPosition(supportingBlock))
				return;
			if (world.GetBlockId(supportingBlock) == 0) // TODO: Air isn't the only thing that can't support some blocks
				OnBlockMined(block, world, updatedBlock, null); // TODO: Consider using a seperate delegate for blocks destroyed through non-player actions
		}

		public static ItemStack[] DefaultGetDropHandler(BlockDescriptor block, World world, Coordinates3D minedBlock)
		{
			return new[] { new ItemStack(block.Id, 1, block.Metadata) };
		}

        #endregion
    }

	public enum SupportDirection
	{
		None,
		Up,
		Down,
		North,
		South,
		East,
		West
	}
}
