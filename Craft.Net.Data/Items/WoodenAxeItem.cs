namespace Craft.Net.Data.Items
{
    public class WoodenAxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 271;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Axe | ToolType.Hoe;  }
        }
    }
}
