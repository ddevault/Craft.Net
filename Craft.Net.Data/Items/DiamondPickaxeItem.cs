namespace Craft.Net.Data.Items
{
    public class DiamondPickaxeItem : ToolItem
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

        public override ToolType ToolType
        {
            get { return ToolType.Pick | ToolType.Diamond; }
        }
    }
}
