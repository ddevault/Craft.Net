namespace Craft.Net.Data.Items
{
    public class BowItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 261;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Other; }
        }
    }
}
