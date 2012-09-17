namespace Craft.Net.Data.Items
{
    public class DiamondShovelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 277;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Shovel | ToolType.Diamond; }
        }
    }
}
