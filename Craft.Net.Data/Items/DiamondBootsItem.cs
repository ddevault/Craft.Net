namespace Craft.Net.Data.Items
{

    public class DiamondBootsItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 313;
            }
        }

        public int ArmorBonus
        {
            get { return 3; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Footwear; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }
    }
}
