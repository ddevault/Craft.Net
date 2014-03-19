using System;

namespace Craft.Net.Logic.Blocks
{
    public class AirBlock : Block
    {
        public static readonly short Id = 0;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            return "minecraft::air";
        }
    }
}