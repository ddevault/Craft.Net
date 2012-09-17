namespace Craft.Net.Data.Items
{

    public class IronSwordItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 267;
            }
        }

        public override int AttackDamage
        {
            get { return 6; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Iron | ToolType.Sword; }
        }
    }
}
