namespace Craft.Net.Data.Items
{

    public class GoldenSwordItem : SwordItem
    {
        public override ushort Id
        {
            get
            {
                return 283;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Gold; }
        }
    }
}
