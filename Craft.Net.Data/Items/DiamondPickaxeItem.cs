namespace Craft.Net.Data.Items
{
    public class DiamondPickaxeItem : PickaxeItem
    {
        public override ushort Id
        {
            get
            {
                return 278;
            }
        }

        public override int AttackDamage
        {
            get { return 5; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}