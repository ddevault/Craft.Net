namespace Craft.Net.Data.Items
{
    public class GoldenHoeItem : HoeItem
    {
        public override ushort Id
        {
            get
            {
                return 294;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Hoe | ToolType.Gold; }
        }
    }
}
