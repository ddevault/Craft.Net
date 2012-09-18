namespace Craft.Net.Data.Items
{
    
    public class ChainChestplateItem : ToolItem, IArmorItem
    {
        public override ushort Id
        {
            get
            {
                return 303;
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
            get { return ToolMaterial.Other; }
        }
    }
}
