namespace Craft.Net.Data.Items
{
    public class GoldenAxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 286;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Axe | ToolType.Gold; }
        }
    }
}
