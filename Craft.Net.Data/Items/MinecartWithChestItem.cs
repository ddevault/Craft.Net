namespace Craft.Net.Data.Items
{
    
    public class MinecartWithChestItem : Item
    {
        public override short Id
        {
            get
            {
                return 342;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}
