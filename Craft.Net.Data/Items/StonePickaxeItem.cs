namespace Craft.Net.Data.Items
{
    public class StonePickaxeItem : ToolItem
    {
        public override ushort Id
        {
            get
            {
                return 274;
            }
        }

        public override int AttackDamage
        {
            get { return 3; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Pick | ToolType.Stone; }
        }
    }
}
