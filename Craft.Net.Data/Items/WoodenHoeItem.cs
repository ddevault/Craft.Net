namespace Craft.Net.Data.Items
{

    public class WoodenHoeItem : HoeItem
    {
        public override ushort Id
        {
            get
            {
                return 290;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Hoe; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Wood; }
        }
    }
}