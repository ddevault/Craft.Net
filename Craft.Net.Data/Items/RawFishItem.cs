namespace Craft.Net.Data.Items
{
    public class RawFishItem : FoodItem
    {
        public override short Id
        {
            get
            {
                return 349;
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
