namespace Craft.Net.Data.Items
{
    public class DiamondHelmetItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 310;
            }
        }

        public override bool IsArmor
        {
            get { return false; }
        }

        public override int ArmorBonus
        {
            get { return 3; }
        }
    }
}
