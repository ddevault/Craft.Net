namespace Craft.Net.Data.Items
{
    public class WoodenPickaxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 270;
            }
        }

        public override int AttackDamage
        {
            get { return 2; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Pick | ToolType.Wood; }
        }
    }
}
