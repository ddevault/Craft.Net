using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic
{
    public static class WorldExtensions
    {
        public static BlockInfo GetBlockInfo(this World world, Coordinates3D coordinates)
        {
            return new BlockInfo(world.GetBlockId(coordinates), world.GetMetadata(coordinates),
                world.GetSkyLight(coordinates), world.GetBlockLight(coordinates));
        }

        public static void SetBlockInfo(this World world, Coordinates3D coordinates, BlockInfo info)
        {
            world.SetBlockId(coordinates, info.BlockId);
            world.SetMetadata(coordinates, info.Metadata);
            world.SetSkyLight(coordinates, info.SkyLight);
            world.SetBlockLight(coordinates, info.BlockLight);
        }

        public static void MineBlock(this World world, Coordinates3D coordinates)
        {
            Block.OnBlockMined(world, coordinates);
        }
        
        public static bool RightClickBlock(this World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo? item)
        {
            return Block.OnBlockRightClicked(world, coordinates, face, cursor, item);
        }
        
        public static void UseItemOnBlock(this World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item)
        {
            Item.OnItemUsedOnBlock(world, coordinates, face, cursor, item);
        }
        
        public static ItemInfo? AsItem(this ItemStack stack)
        {
            if (stack.Empty)
                return null;
            return new ItemInfo(stack.Id, stack.Metadata);
        }
    }
}