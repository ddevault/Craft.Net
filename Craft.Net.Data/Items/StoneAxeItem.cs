namespace Craft.Net.Data.Items
{
   public class StoneAxeItem : AxeItem
   {
      public override ushort Id
      {
         get
         {
            return 275;
         }
      }

      public override int AttackDamage
      {
         get { return 4; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Stone; }
      }
   }
}