namespace Craft.Net.Data.Items
{

    public class GoldenSwordItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 283;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Sword | ToolType.Gold; }
        }
    }
}
