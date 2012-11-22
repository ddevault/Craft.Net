namespace Craft.Net.Data.Items
{
    public class RottenFleshItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 367;
            }
        }

        public override int FoodPoints
        {
            get { return 4; }
        }

        public override float Saturation
        {
            get { return 0.8f; }
        }
    }
}