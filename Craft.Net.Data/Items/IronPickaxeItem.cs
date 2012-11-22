namespace Craft.Net.Data.Items
{
    public class IronPickaxeItem : PickaxeItem
    {
        public override ushort Id
        {
            get
            {
                return 257;
            }
        }

        public override int AttackDamage
        {
            get { return 4; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Iron; }
        }
    }
}