namespace Craft.Net.Data.Items
{
    public class DiamondLeggingsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 312;
            }
        }

        public override bool IsArmor
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 6; }
        }
    }
}
