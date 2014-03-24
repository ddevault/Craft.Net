using System;

namespace Craft.Net.Logic.Blocks
{
    public class WheatBlock : Block
    {
        public static readonly short Id = 59;

        protected override string Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
            return "minecraft:seeds";
        }
    }
}