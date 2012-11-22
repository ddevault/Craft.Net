namespace Craft.Net.Data.Items
{
    public class MilkItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 335;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}