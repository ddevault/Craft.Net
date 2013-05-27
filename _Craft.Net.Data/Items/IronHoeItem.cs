namespace Craft.Net.Data.Items
{
    public class IronHoeItem : HoeItem
    {
        public override short Id
        {
            get
            {
                return 292;
            }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Hoe; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Iron; }
        }
    }
}
