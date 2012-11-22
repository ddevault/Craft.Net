namespace Craft.Net.Data.Items
{
    public class SpiderEyeItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 375;
            }
        }

        public override int FoodPoints
        {
            get { return 2; }
        }

        public override float Saturation
        {
            get { return 3.2f; }
        }
    }
}