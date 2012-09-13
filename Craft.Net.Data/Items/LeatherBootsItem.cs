namespace Craft.Net.Data.Items
{
    
    public class LeatherBootsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 301;
            }
        }

        public override bool IsArmor // TODO: Change to ArmorSlots enumerable
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 1; }
        }
    }
}
