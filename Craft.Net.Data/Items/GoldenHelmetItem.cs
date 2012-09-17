namespace Craft.Net.Data.Items
{
    public class GoldenHelmetItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 314;
            }
        }

        public int ArmorBonus
        {
            get { return 2; }
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
