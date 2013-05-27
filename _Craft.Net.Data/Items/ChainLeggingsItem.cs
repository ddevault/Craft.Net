namespace Craft.Net.Data.Items
{

    public class ChainLeggingsItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 304;
            }
        }

        public int ArmorBonus
        {
            get { return 4; }
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
