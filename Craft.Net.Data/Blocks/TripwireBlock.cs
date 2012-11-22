using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public class TripwireBlock : Block
   {
      public override ushort Id
      {
         get { return 132; }
      }

      public override BoundingBox? BoundingBox
      {
         get { return null; }
      }

      public override bool GetDrop(ToolItem tool, out Slot[] drop)
      {
         drop = new[] { new Slot((ushort)new StringItem(), 1) };
         return true;
      }
   }
}