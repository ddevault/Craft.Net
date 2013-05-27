namespace Craft.Net.Data.Items
{
    public class BoatItem : Item
    {
        public override short Id
        {
            get
            {
                return 333;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}
