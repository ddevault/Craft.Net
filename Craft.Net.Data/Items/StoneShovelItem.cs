namespace Craft.Net.Data.Items
{
    public class StoneShovelItem : ShovelItem
    {
        public override ushort Id
        {
            get
            {
                return 273;
            }
        }

        public override int AttackDamage
        {
            get { return 2; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Stone; }
        }
    }
}