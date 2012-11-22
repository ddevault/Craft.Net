namespace Craft.Net.Data.Items
{
    public class GoldenAxeItem : AxeItem
    {
        public override ushort Id
        {
            get
            {
                return 286;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Gold; }
        }
    }
}