using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public class TallGrassBlock : Block
   {
      public override ushort Id
      {
         get { return 31; }
      }

      public override BoundingBox? BoundingBox
      {
         get { return null; }
      }

      public override bool RequiresSupport
      {
         get { return true; }
      }

      public override Vector3 SupportDirection
      {
         get { return Vector3.Down; }
      }

      public override bool GetDrop(ToolItem tool, out Slot[] drop)
      {
         drop = new[] { new Slot((ushort)new SeedsItem(), 1) };
         return DataUtility.Random.Next(0, 5) == 0; // TODO: Find exact value
      }
   }
}