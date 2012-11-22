namespace Craft.Net.Data.Items
{
   public class IronAxeItem : AxeItem
   {
      public override ushort Id
      {
         get
         {
            return 258;
         }
      }

      public override int AttackDamage
      {
         get { return 5; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Iron; }
      }
   }
}