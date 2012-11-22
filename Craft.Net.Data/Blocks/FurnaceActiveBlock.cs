using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Blocks
{
   public class FurnaceActiveBlock : FurnaceBlock
   {
      public override ushort Id
      {
         get { return 62; }
      }

      public override bool GetDrop(Items.ToolItem tool, out Slot[] drop)
      {
         drop = new[] { new Slot((ushort)new FurnaceBlock(), 1) };
         return true;
      }
   }
}