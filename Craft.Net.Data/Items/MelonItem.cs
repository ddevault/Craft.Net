namespace Craft.Net.Data.Items
{
   public class MelonItem : FoodItem
   {
      public override ushort Id
      {
         get
         {
            return 360;
         }
      }

      public override int FoodPoints
      {
         get { return 2; }
      }

      public override float Saturation
      {
         get { return 1.2f; }
      }
   }
}