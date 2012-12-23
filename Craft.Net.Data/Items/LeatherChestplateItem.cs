namespace Craft.Net.Data.Items
{
    public class LeatherChestplateItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 299;
            }
        }

        public int ArmorBonus
        {
            get { return 3; }
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
            get { return ToolMaterial.Other; }
        }
    }
}
