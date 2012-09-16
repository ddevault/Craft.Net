namespace Craft.Net.Data.Items
{
    
    public class MinecartWithChestItem : Item
    {
        public override ushort Id
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
