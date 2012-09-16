namespace Craft.Net.Data.Items
{
    public class LeatherHelmetItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 298;
            }
        }

        public int ArmorBonus
        {
            get { return 1; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Headgear; }
        }
    }
}
