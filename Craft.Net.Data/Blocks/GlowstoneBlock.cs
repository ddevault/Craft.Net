using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public class GlowstoneBlock : Block
   {
      public override ushort Id
      {
         get { return 89; }
      }

      public override double Hardness
      {
         get { return 0.3; }
      }

      public override bool GetDrop(ToolItem tool, out Slot[] drop)
      {
         drop = new[] { new Slot((ushort)new GlowstoneDustItem(), (byte)DataUtility.Random.Next(2, 4)) };
         return true;
      }
   }
}