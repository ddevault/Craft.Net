namespace Craft.Net.Data.Items
{

    public class IronSwordItem : SwordItem
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

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Iron; }
        }
    }
}