namespace Craft.Net.Data.Items
{
    public class LeatherLeggingsItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 300;
            }
        }

        public int ArmorBonus
        {
            get { return 2; }
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
            get { return ToolMaterial.Other; }
        }
    }
}
