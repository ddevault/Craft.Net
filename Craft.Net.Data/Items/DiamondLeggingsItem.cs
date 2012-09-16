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
    }
}
