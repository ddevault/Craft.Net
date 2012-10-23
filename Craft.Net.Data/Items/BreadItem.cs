namespace Craft.Net.Data.Items
{
    
    public class BreadItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 297;
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
