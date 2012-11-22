namespace Craft.Net.Data.Items
{
    public class GoldenLeggingsItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 316;
            }
        }

        public int ArmorBonus
        {
            get { return 3; }
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
            get { return ToolMaterial.Gold; }
        }
    }
}