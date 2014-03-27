using System;

namespace Craft.Net.Logic.Blocks
{
    public class YellowFlowerBlock : Block
    {
        public static readonly short Id = 37;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            return "minecraft:yellow_flower";
        }
    }

    public class FlowerBlock : Block
    {
        public static readonly short Id = 38;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            return "minecraft:red_flower";
        }
    }
}

