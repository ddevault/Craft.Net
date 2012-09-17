namespace Craft.Net.Data.Items
{

    public class StoneSwordItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 272;
            }
        }

        public override int AttackDamage
        {
            get { return 5; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Sword | ToolType.Stone; }
        }
    }
}
