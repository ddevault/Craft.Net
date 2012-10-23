namespace Craft.Net.Data.Items
{
    public class GoldenAppleItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 322;
            }
        }

        public override int FoodPoints
        {
            get { return 4; }
        }

        public override float Saturation
        {
            get { return 9.6f; }
        }
    }
}
