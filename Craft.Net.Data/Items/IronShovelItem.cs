namespace Craft.Net.Data.Items
{
    public class IronShovelItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 256;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Shovel | ToolType.Iron; }
        }
    }
}
