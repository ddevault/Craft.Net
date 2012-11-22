namespace Craft.Net.Data.Items
{
    public class CookedPorkchopItem : FoodItem
    {
        public override ushort Id
        {
            get
            {
                return 320;
            }
        }

        public override int FoodPoints
        {
            get { return 8; }
        }

        public override float Saturation
        {
            get { return 12.8f; }
        }
    }
}