namespace Craft.Net.Data.Items
{
   public class WoodenShovelItem : ShovelItem
   {
      public override ushort Id
      {
         get
         {
            return 269;
         }
      }

      public override int AttackDamage
      {
         get { return 1; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Wood; }
      }
   }
}