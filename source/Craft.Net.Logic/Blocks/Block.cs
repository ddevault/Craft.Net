using Craft.Net.Anvil;
using Craft.Net.Logic.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public static class Block
    {
        public static void Initialize(ItemLogicDescriptor descriptor)
        {
            descriptor.ItemUsedOnBlock = OnItemUsedOnBlock;
        }

        public static void OnItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            // TODO: Right click block, on block placed, etc
            if ((clickedBlock + clickedSide).Y >= 0 && (clickedBlock + clickedSide).Y <= Chunk.Height)
            {
                world.SetBlockId(clickedBlock + clickedSide, item.Id);
                world.SetMetadata(clickedBlock + clickedSide, (byte)item.Metadata);
            }
        }
    }
}
