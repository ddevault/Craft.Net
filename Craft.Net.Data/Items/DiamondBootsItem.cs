namespace Craft.Net.Data.Items
{
    
    public class DiamondBootsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 313;
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
