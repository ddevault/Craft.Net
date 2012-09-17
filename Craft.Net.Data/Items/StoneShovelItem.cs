namespace Craft.Net.Data.Items
{
    public class StoneShovelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 273;
            }
        }

        public override int AttackDamage
        {
            get { return 2; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Shovel | ToolType.Stone; }
        }
    }
}
