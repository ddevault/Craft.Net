namespace Craft.Net.Data.Items
{

    public class DiamondChestplateItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 311;
            }
        }

        public int ArmorBonus
        {
            get { return 8; }
        }

        public ArmorSlot ArmorSlot
        {
            get { return ArmorSlot.Chestplate; }
        }

        public override ToolType ToolType
        {
            get { return ToolType.Other; }
        }

        public override ToolMaterial ToolMaterial
        {
            get { return ToolMaterial.Diamond; }
        }
    }
}
