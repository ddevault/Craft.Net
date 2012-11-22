namespace Craft.Net.Data.Items
{

    public class CookedChickenItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 366;
            }
        }

        public override int FoodPoints
        {
            get { return 6; }
        }

        public override float Saturation
        {
            get { return 7.2f; }
        }
    }
}