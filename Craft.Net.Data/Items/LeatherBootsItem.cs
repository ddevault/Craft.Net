namespace Craft.Net.Data.Items
{

    public class LeatherBootsItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 301;
            }
        }

        public int ArmorBonus
        {
            get { return 1; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Footwear; }
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