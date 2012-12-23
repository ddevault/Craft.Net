namespace Craft.Net.Data.Items
{
    public class DiamondShovelItem : ShovelItem
    {
        public override short Id
        {
            get
            {
                return 277;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}
