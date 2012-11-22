using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
   public abstract class PickaxeItem : ToolItem
   {
      public override bool IsEfficient(Block block)
      {
         return block.Transparency == Transparency.Opaque;
      }

      public override ToolType ToolType
      {
         get { return ToolType.Pick; }
      }
   }
}