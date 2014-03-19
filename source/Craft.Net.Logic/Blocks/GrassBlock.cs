using System;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class GrassBlock : Block
    {
        public static readonly short Id = 2;

        protected override string Initialize()
        {
            SetDropHandler(Id, (world, coordinates, info) => new[] { new ItemStack(DirtBlock.Id) });
            return "minecraft:grass";
        }
    }
}