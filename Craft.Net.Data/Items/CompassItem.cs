namespace Craft.Net.Data.Items
{
    public class CompassItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 345;
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
