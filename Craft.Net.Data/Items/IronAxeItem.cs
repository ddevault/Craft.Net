namespace Craft.Net.Data.Items
{
    public class IronAxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 258;
            }
        }

        public override int AttackDamage
        {
            get { return 5; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Axe | ToolType.Iron; }
        }
    }
}
