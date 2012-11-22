using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Items
{
   public class BakedPotatoItem : FoodItem
   {
      public override ushort Id
      {
         get { return 393; }
      }

      public override int FoodPoints
      {
         get { return 6; }
      }

      public override float Saturation
      {
         get { return 8; }
      }
   }
}