using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
   public abstract class ShovelItem : ToolItem
   {
      public override bool IsEfficient(Block block)
      {
         return block is ClayBlock ||
            block is DirtBlock ||
            block is GrassBlock ||
            block is GravelBlock ||
            block is MyceliumBlock ||
            block is SandBlock ||
            block is SnowBlock ||
            block is SnowfallBlock ||
            block is SoulSandBlock;
         }

         public override ToolType ToolType
         {
            get { return ToolType.Axe; }
         }
      }
}