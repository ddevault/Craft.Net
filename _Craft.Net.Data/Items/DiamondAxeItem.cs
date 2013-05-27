namespace Craft.Net.Data.Items
{
    public class DiamondAxeItem : AxeItem
    {
        public override short Id
        {
            get
            {
                return 279;
            }
        }

        public override int AttackDamage
        {
            get { return 6; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}
