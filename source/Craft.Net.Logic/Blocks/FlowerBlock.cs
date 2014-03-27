using System;

namespace Craft.Net.Logic.Blocks
{
    public class YellowFlowerBlock : Block
    {
        public static readonly short Id = 37;
        public override short BlockId { get { return Id; } }

        public YellowFlowerBlock() : base("minecraft:yellow_flower")
        {
            base.SetBoundingBox(null);
        }
    }

    public class FlowerBlock : Block
    {
        public static readonly short Id = 38;
        public override short BlockId { get { return Id; } }

        public FlowerBlock() : base("minecraft:red_flower")
        {
            base.SetBoundingBox(null);
        }
    }
}

