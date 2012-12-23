namespace Craft.Net.Data.Items
{
    public class AppleItem : FoodItem
    {
        public override short Id
        {
            get
            {
                return 260;
            }
        }

        public override int FoodPoints
        {
            get { return 2; }
        }

        public override float Saturation
        {
            get { return 2.4f; }
        }
    }
}
