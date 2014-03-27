using System;

namespace Craft.Net.Logic.Blocks
{
    public class WheatBlock : Block
    {
        public static readonly short Id = 59;
        public override short BlockId { get { return Id; } }

        public WheatBlock() : base("minecraft:seeds")
        {
            base.SetBoundingBox(null);
        }
    }
}