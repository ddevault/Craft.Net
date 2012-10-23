namespace Craft.Net.Data.Items
{
    public class CookieItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 357;
            }
        }

        public override int FoodPoints
        {
            get { return 2; }
        }

        public override float Saturation
        {
            get { return 0.4f; }
        }
    }
}
