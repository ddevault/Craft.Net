using System;

namespace Craft.Net.Logic.Blocks
{
    public class AirBlock : Block
    {
        public static readonly short Id = 0;
        public override short BlockId { get { return Id; } }

        public AirBlock() : base("minecraft:air")
        {
            base.SetBoundingBox(null);
        }
    }
}