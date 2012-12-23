namespace Craft.Net.Data.Items
{
    public class DiamondHoeItem : HoeItem
    {
        public override short Id
        {
            get
            {
                return 293;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Hoe; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}
