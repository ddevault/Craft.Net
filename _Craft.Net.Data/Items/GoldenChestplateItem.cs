namespace Craft.Net.Data.Items
{

    public class GoldenChestplateItem : ToolItem, IArmorItem
    {
        public override short Id
        {
            get
            {
                return 315;
            }
        }

        public int ArmorBonus
        {
            get { return 5; }
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
            get { return ToolMaterial.Gold; }
        }
    }
}
