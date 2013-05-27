namespace Craft.Net.Data.Items
{
    public class StonePickaxeItem : PickaxeItem
    {
        public override short Id
        {
            get
            {
                return 274;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Stone; }
        }
    }
}
