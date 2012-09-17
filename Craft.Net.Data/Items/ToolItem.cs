using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    [Flags]
    public enum ToolType
    {
        None = 0,
        Pick = 1,
        Axe = 2,
        Shovel = 4,
        Sword = 8,
        Hoe = 16,
        Other = 32,
        Wood = 64,
        Stone = 128,
        Iron = 256,
        Gold = 512,
        Diamond = 1024,
        All = Pick | Axe | Shovel | Sword | Other | Wood | Stone | Iron | Gold | Diamond
    }

    public abstract class ToolItem : Item
    {
        public override byte MaximumStack
        {
            get { return 1; }
        }

        public abstract ToolType ToolType { get; }

        public bool CanHarvest(Block block)
        {
            return block.CanHarvest(this);
        }
    }
}
