namespace Craft.Net.Data.Items
{
   public class FishingRodItem : ToolItem
   {
      public override ushort Id
      {
         get
         {
            return 346;
         }
      }

      public override ToolType ToolType
      {
         get { return ToolType.Other; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Other; }
      }
   }
}