namespace Craft.Net.Data.Items
{
    
    public class ChainChestplateItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 303;
            }
        }

        public override bool IsArmor
        {
            get { return true; }
        }

        public override int ArmorBonus
        {
            get { return 5; }
        }
    }
}
