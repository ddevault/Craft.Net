namespace Craft.Net.Data.Items
{
    
    public class ChainBootsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 305;
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
