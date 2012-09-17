namespace Craft.Net.Data.Items
{
    public class WoodenShovelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 269;
            }
        }

        public override int AttackDamage
        {
            get { return 1; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Shovel | ToolType.Wood; }
        }
    }
}
