namespace Craft.Net.Data.Items
{
   public class WoodenPickaxeItem : PickaxeItem
   {
      public override ushort Id
      {
         get
         {
            return 270;
         }
      }

      public override int AttackDamage
      {
         get { return 2; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Wood; }
      }
   }
}