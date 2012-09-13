namespace Craft.Net.Data.Items
{
    public class LeatherHelmetItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 298;
            }
        }

        public override bool IsArmor
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 1; }
        }
    }
}
