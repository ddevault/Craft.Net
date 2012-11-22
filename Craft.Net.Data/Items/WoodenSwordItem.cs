namespace Craft.Net.Data.Items
{
    public class WoodenSwordItem : SwordItem
    {
        public override ushort Id
        {
            get
            {
                return 268;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Wood; }
        }
    }
}