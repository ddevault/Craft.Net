namespace Craft.Net.Data.Items
{
    public class CookedFishItem : FoodItem
    {
        public override short Id
        {
            get
            {
                return 350;
            }
        }

        public override int FoodPoints
        {
            get { return 5; }
        }

        public override float Saturation
        {
            get { return 6; }
        }
    }
}
