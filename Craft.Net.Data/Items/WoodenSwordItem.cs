namespace Craft.Net.Data.Items
{
    public class WoodenSwordItem : ToolItem
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

        public override ToolType ToolType
        {
            get { return ToolType.Sword | ToolType.Wood; }
        }
    }
}
