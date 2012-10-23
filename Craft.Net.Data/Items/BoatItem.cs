namespace Craft.Net.Data.Items
{
    public class BoatItem : Item
    {
        public override ushort Id
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
