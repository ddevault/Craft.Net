namespace Craft.Net.Data.Items
{
    public class MusicDiscItem : Item
    {
        public override ushort Id
        {
            get
            {
                return 2266;
            }
        }

        public override byte MaximumStack
        {
            get { return 1; }
        }
    }
}