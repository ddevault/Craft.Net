using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
    public abstract class AxeItem : ToolItem
    {
        public override bool IsEfficient(Block block)
        {
            return block is TrapDoorBlock ||
                   block is WoodenDoorBlock ||
                   block is ChestBlock ||
                   block is CraftingTableBlock ||
                   block is FenceBlock ||
                   block is FenceGateBlock ||
                   block is JukeboxBlock ||
                   block is LogBlock ||
                   block is WoodenSlabBlock ||
                   block is WoodenPlanksBlock ||
                   block is BirchWoodStairBlock ||
                   block is SpruceWoodStairBlock ||
                   block is OakStairBlock ||
                   block is JungleWoodStairBlock ||
                   block is BookshelfBlock ||
                   block is PumpkinBlock ||
                   block is SignBlock ||
                   block is NoteBlock ||
                   block is WoodenPressurePlateBlock ||
                   block is HugeBrownMushroomBlock ||
                   block is HugeRedMushroomBlock;
        }

        public override ToolType ToolType
        {
            get { return ToolType.Axe; }
        }
    }
}
