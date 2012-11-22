namespace Craft.Net.Data.Items
{
   public class IronShovelItem : ShovelItem
   {
      public override ushort Id
      {
         get
         {
            return 256;
         }
      }

      public override int AttackDamage
      {
         get { return 3; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Iron; }
      }
   }
}