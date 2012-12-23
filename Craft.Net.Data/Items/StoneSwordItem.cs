namespace Craft.Net.Data.Items
{

    public class StoneSwordItem : SwordItem
    {
        public override short Id
        {
            get
            {
                return 272;
            }
        }

        public override int AttackDamage
        {
            get { return 5; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Stone; }
        }
    }
}
