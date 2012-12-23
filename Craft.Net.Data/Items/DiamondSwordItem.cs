namespace Craft.Net.Data.Items
{
    public class DiamondSwordItem : SwordItem
    {
        public override short Id
        {
            get
            {
                return 276;
            }
        }

        public override int AttackDamage
        {
            get { return 7; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}
