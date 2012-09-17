namespace Craft.Net.Data.Items
{
    public class DiamondSwordItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 276;
            }
        }

        public override int AttackDamage
        {
            get { return 7; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Sword | ToolType.Diamond; }
        }
    }
}
