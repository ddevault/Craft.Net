namespace Craft.Net.Data.Items
{
    public class DiamondAxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 279;
            }
        }

        public override int AttackDamage
        {
            get { return 6; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Axe | ToolType.Diamond; }
        }
    }
}
