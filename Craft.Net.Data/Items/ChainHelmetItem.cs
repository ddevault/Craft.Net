namespace Craft.Net.Data.Items
{
    
    public class ChainHelmetItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 302;
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
