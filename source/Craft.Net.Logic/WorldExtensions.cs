using Craft.Net.Anvil;
using Craft.Net.Logic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Logic
{
    public static class WorldExtensions
    {
        public static void SetBlock(this World world, Coordinates3D coordinates, BlockDescriptor value)
        {
            world.SetBlockId(coordinates, value.Id);
            world.SetMetadata(coordinates, value.Metadata);
            world.SetSkyLight(coordinates, value.SkyLight);
            world.SetBlockLight(coordinates, value.BlockLight);
        }

        public static BlockDescriptor GetBlock(this World world, Coordinates3D coordinates)
        {
            return new BlockDescriptor(
                world.GetBlockId(coordinates), world.GetMetadata(coordinates),
                world.GetBlockLight(coordinates), world.GetSkyLight(coordinates));
        }
    }
}
