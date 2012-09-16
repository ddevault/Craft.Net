namespace Craft.Net.Data.Items
{

    public class GoldenBootsItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 317;
            }
        }

        public int ArmorBonus
        {
            get { return 1; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Footwear; }
        }
    }
}
