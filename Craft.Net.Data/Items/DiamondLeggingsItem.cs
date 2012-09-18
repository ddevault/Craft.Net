namespace Craft.Net.Data.Items
{
    public class DiamondLeggingsItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 312;
            }
        }

        public int ArmorBonus
        {
            get { return 6; }
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
            get { return ToolMaterial.Diamond; }
        }
    }
}
