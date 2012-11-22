namespace Craft.Net.Data.Items
{

   public class StoneHoeItem : HoeItem
   {
      public override ushort Id
      {
         get
         {
            return 291;
         }
      }

      public override ToolType ToolType
      {
         get { return ToolType.Hoe; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Stone; }
      }
   }
}