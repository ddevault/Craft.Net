namespace Craft.Net.Data.Items
{
    public class IronChestplateItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 307;
            }
        }

        public int ArmorBonus
        {
            get { return 6; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Chestplate; }
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
