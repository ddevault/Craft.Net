namespace Craft.Net.Data.Items
{
    public class ClockItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 347;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }
    }
}
