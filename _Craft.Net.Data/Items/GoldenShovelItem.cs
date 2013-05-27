namespace Craft.Net.Data.Items
{
    public class GoldenShovelItem : ShovelItem
    {
        public override short Id
        {
            get
            {
                return 284;
            }
        }

        public override int AttackDamage
        {
            get { return 1; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Gold; }
        }
    }
}
