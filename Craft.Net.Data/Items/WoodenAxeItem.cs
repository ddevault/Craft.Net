namespace Craft.Net.Data.Items
{
   public class WoodenAxeItem : AxeItem
   {
      public override ushort Id
      {
         get
         {
            return 271;
         }
      }

      public override int AttackDamage
      {
         get { return 3; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Wood; }
      }
   }
}