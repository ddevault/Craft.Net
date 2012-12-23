namespace Craft.Net.Data.Items
{
    public class ClockItem : ToolItem
    {
        public override short Id
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

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Other; }
        }
    }
}
