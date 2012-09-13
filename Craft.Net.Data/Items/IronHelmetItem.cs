namespace Craft.Net.Data.Items
{
    public class IronHelmetItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 306;
            }
        }

        public override bool IsArmor
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 2; }
        }
    }
}
