namespace Craft.Net.Data.Items
{
    public class GoldenHelmetItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 314;
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
