namespace Craft.Net.Data.Items
{
    public class GoldenHoeItem : HoeItem
    {
        public override short Id
        {
            get
            {
                return 294;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Hoe; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Gold; }
        }
    }
}
