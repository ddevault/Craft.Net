using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Data.Items
{
   public class PotatoItem : FoodItem
   {
      public override ushort Id
      {
         get { return 392; }
      }

      public override void OnItemUsedOnBlock(World world, Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, Entities.Entity usedBy)
      {
         if (world.GetBlock(clickedBlock + clickedSide) == 0)
            world.SetBlock(clickedBlock + clickedSide, new PotatoBlock());
         }

         public override int FoodPoints
         {
            get { return 1; }
         }

         public override float Saturation
         {
            get { return 0.8f; }
         }
      }
}