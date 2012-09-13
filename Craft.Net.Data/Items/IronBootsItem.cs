namespace Craft.Net.Data.Items
{
    
    public class IronBootsItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 309;
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
