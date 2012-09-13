namespace Craft.Net.Data.Items
{
    public class GoldenLeggingsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 316;
            }
        }

        public override bool IsArmor
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 3; }
        }
    }
}
