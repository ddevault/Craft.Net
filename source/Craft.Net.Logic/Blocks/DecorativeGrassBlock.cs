using System;
using Craft.Net.Common;

namespace Craft.Net.Logic.Blocks
{
    public class DecorativeGrassBlock : Block
    {
        public static readonly short Id = 31;
        public override short BlockId { get { return Id; } }

        public DecorativeGrassBlock() : base("minecraft:tallgrass")
        {
            base.SetBoundingBoxHandler (BoundingBox);
               base.SetDropHandler ((world, coordinates, info) => new[] { new ItemStack(295) });
        }
        
        private BoundingBox? BoundingBox(BlockInfo info)
        {
            return null;   
        }
    }

    public class TallGrassBlock : Block
    {
        public static readonly short Id = 175;
        public override short BlockId { get { return Id; } }

        public TallGrassBlock() : base("minecraft:tallgrass")
        {
            base.SetBoundingBoxHandler(BoundingBox);
        }
        
        private BoundingBox? BoundingBox(BlockInfo info)
        {
            return null;   
        }
    }
}
