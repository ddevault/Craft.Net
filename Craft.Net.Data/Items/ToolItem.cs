using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
    public enum ToolType
    {
        None,
        Pick,
        Axe,
        Shovel,
        Sword,
        Hoe,
        Other
    }

    public enum ToolMaterial
    {
        Wood,
        Stone,
        Iron,
        Gold,
        Diamond,
        Other
    }

    public abstract class ToolItem : Item
    {
        public override byte MaximumStack
        {
            get { return 1; }
        }

        public abstract ToolType ToolType { get; }
        public abstract ToolMaterial ToolMaterial { get; }

        public bool CanHarvest(Block block)
        {
            return block.CanHarvest(this);
        }

        public virtual bool IsEfficient(Block block)
        {
            return false;
        }
    }
}
