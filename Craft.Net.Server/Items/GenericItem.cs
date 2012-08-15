namespace Craft.Net.Server.Items
{
    public class GenericItem : Item
    {
        private readonly short internalItemID;

        public GenericItem(short ItemID)
        {
            internalItemID = ItemID;
        }

        public override short ItemID
        {
            get { return internalItemID; }
        }
    }
}