using System;

namespace Craft.Net.Logic.Blocks
{
    public class DecorativeGrassBlock : Block
    {
        public static readonly short Id = 31;
        public override short BlockId { get { return Id; } }

        public DecorativeGrassBlock() : base("minecraft:tallgrass")
        {
            base.SetBoundingBox(null);
            //TODO: Once items are implemented, we need to drop seeds here
            //SetDropHandler(Id, (world, coordinates, info) => new[] { new ItemStack(ItemSeeds.Id) });
        }
    }

    public class TallGrassBlock : Block
    {
        public static readonly short Id = 175;
        public override short BlockId { get { return Id; } }

        public TallGrassBlock() : base("minecraft:tallgrass")
        {
            base.SetBoundingBox(null);
        }
    }
}
