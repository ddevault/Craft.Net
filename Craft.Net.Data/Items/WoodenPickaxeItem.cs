namespace Craft.Net.Data.Items
{
    public class WoodenPickaxeItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 270;
            }
        }

        public override int AttackDamage
        {
            get { return 2; }
        }
    }
}
