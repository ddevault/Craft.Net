namespace Craft.Net.Data.Items
{
    public class GenericItem : Item
    {
        private readonly short internalItemID;

        public GenericItem(short itemID)
        {
            internalItemID = itemID;
        }

        public override short ItemID
        {
            get { return internalItemID; }
        }
    }
}