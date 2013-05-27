namespace Craft.Net.Data.Items
{
    public class IronHelmetItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 306;
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
            get { return ToolMaterial.Iron; }
        }
    }
}
