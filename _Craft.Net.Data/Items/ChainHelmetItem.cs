namespace Craft.Net.Data.Items
{

    public class ChainHelmetItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 302;
            }
        }

        public int ArmorBonus
        {
            get { return 2; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Headgear; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Other; }
        }
    }
}
