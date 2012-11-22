using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Items;

namespace Craft.Net.Data.Blocks
{
   public class PotatoBlock : Block
   {
      public override ushort Id
      {
         get { return 142; }
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
         if (DataUtility.Random.Next(100) == 0)
         {
            drop = new[] { new Slot((ushort)new PotatoItem(), (byte)DataUtility.Random.Next(1, 4)),
               new Slot((ushort)new PoisonousPotatoItem(), 1) };
            }
            else
            {
               drop = new[] { new Slot((ushort)new PotatoItem(), (byte)DataUtility.Random.Next(1, 4)) };
            }
            return true;
         }
      }
}