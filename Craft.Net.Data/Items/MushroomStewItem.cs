namespace Craft.Net.Data.Items
{
    public class MushroomStewItem : FoodItem
    {
        public override short Id
        {
            get
            {
                return 282;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }

        public override int FoodPoints
        {
            get { return 8; }
        }

        public override float Saturation
        {
            get { return 9.6f; }
        }
    }
}
