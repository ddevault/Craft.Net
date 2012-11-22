namespace Craft.Net.Data.Items
{
   public class IronLeggingsItem : ToolItem, IArmorItem
   {
      public override ushort Id
      {
         get
         {
            return 308;
         }
      }

      public int ArmorBonus
      {
         get { return 5; }
      }

      public ArmorSlot ArmorSlot
      {
         get { return ArmorSlot.Pants; }
      }

      public override ToolType ToolType
      {
         get { return ToolType.Other; }
      }

      public override ToolMaterial ToolMaterial
      {
         get { return ToolMaterial.Iron; }
      }
   }
}