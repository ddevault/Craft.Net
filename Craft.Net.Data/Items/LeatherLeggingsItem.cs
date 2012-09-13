namespace Craft.Net.Data.Items
{
    public class LeatherLeggingsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 300;
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
