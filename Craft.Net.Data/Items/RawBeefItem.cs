namespace Craft.Net.Data.Items
{
    public class RawBeefItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 363;
            }
        }

        public override int FoodPoints
        {
            get { return 3; }
        }

        public override float Saturation
        {
            get { return 1.8f; }
        }
    }
}