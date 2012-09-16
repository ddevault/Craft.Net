namespace Craft.Net.Data.Items
{
    public class LeatherLeggingsItem : ToolItem, IArmorItem
    {
        public override ushort Id
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
    }
}
