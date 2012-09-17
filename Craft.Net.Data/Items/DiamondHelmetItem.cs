namespace Craft.Net.Data.Items
{
    public class DiamondHelmetItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 310;
            }
        }

        public int ArmorBonus
        {
            get { return 3; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Headgear; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }
    }
}
