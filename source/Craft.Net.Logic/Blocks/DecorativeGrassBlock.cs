using System;

namespace Craft.Net.Logic.Blocks
{
    public class DecorativeGrassBlock : Block
    {
        public static readonly short Id = 31;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            //TODO: Once items are implemented, we need to drop seeds here
            //SetDropHandler(Id, (world, coordinates, info) => new[] { new ItemStack(ItemSeeds.Id) });
            return "minecraft:tallgrass";
        }
    }

    public class TallGrassBlock : Block
    {
        public static readonly short Id = 175;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            return "minecraft:tallgrass";
        }
    }
}
