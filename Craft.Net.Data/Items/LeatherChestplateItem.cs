namespace Craft.Net.Data.Items
{
    public class LeatherChestplateItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 299;
            }
        }

        public int ArmorBonus
        {
            get { return 3; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Chestplate; }
        }
    }
}
