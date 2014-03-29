using System;

namespace Craft.Net.Logic.Items
{
    public abstract class PickaxeItem : Item
    {
        public PickaxeItem(string name, ItemMaterial material)
            : base(name, material, Craft.Net.Logic.ToolType.Pickaxe)
        {
        }
    }
    
    public class WoodenPickaxeItem : PickaxeItem
    {
        public override short ItemId { get { return 270; } }

        public WoodenPickaxeItem() : base("minecraft:wooden_pickaxe", ItemMaterial.Wood)
        {
        }
    }
    
    public class StonePickaxeItem : PickaxeItem
    {
        public override short ItemId { get { return 274; } }

        public StonePickaxeItem() : base("minecraft:stone_pickaxe", ItemMaterial.Stone)
        {
        }
    }

    public class IronPickaxeItem : PickaxeItem
    {
        public override short ItemId { get { return 257; } }

        public IronPickaxeItem () : base("minecraft:iron_pickaxe", ItemMaterial.Iron)
        {
        }
    }

    public class GoldenPickaxeItem : PickaxeItem
    {
        public override short ItemId { get { return 285; } }

        public GoldenPickaxeItem() : base("minecraft:golden_pickaxe", ItemMaterial.Gold)
        {
        }
    }

    public class DiamondPickaxeItem : PickaxeItem
    {
        public override short ItemId { get { return 278; } }

        public DiamondPickaxeItem() : base("minecraft:diamond_pickaxe", ItemMaterial.Diamond)
        {
        }
    }
}