namespace Craft.Net.Data.Items
{
    
    public class IronChestplateItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 307;
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
