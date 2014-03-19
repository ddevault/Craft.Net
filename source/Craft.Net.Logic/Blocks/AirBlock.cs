using System;

namespace Craft.Net.Logic.Blocks
{
    public class AirBlock : Block
    {
        public static short Id = 0;

        public override void Initialize()
        {
            LogicManager.SetBoundingBox(Id, null);
        }
    }
}