using Craft.Net.Data.Blocks;
using System;

namespace Craft.Net.Data.Items
{

    public class ShearsItem : ToolItem
    {
        public static Type[] DamagingBlocks = new Type[]
            {
                typeof(CobwebBlock),
                typeof(LeavesBlock),
                typeof(TallGrassBlock), 
                typeof(TripwireBlock), 
                typeof(VineBlock)
            };

        public override short Id
        {
            get
            {
                return 359;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Other; }
        }
    }
}
