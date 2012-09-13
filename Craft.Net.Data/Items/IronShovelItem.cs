namespace Craft.Net.Data.Items
{
    public class IronShovelItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 256;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }
    }
}
