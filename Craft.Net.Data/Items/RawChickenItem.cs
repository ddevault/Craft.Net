namespace Craft.Net.Data.Items
{
    public class RawChickenItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 365;
            }
        }

        public override int FoodPoints
        {
            get { return 2; }
        }

        public override float Saturation
        {
            get { return 1.2f; }
        }
    }
}