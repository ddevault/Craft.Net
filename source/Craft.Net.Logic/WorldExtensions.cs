using System;
using Craft.Net.Anvil;

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
            LogicManager.OnBlockMined(world, coordinates);
        }
    }
}