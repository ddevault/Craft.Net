namespace Craft.Net.Data.Items
{
    public class GoldenShovelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 284;
            }
        }

        public override int AttackDamage
        {
            get { return 1; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Shovel | ToolType.Diamond; }
        }
    }
}
