using System;

namespace Craft.Net.Logic.Blocks
{
    public class DirtBlock : Block
    {
        public static readonly short Id = 3;

        protected override string Initialize()
        {
            return "minecraft:dirt";
        }
    }
}