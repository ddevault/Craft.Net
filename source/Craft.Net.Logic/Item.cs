using System;
using System.Reflection;
using System.Collections.Generic;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic
{
    public abstract class Item
    {
        public delegate void ItemUsedOnBlockHandler(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item);
        public delegate bool ToolEfficiencyHandler(short blockId);
        
        private static Dictionary<short, string> ItemNames { get; set; }
        private static Dictionary<short, ToolType> ToolTypes { get; set; }
        private static Dictionary<short, ItemMaterial> ItemMaterials { get; set; }
        private static Dictionary<short, ToolEfficiencyHandler> ToolEfficiencyHandlers { get; set; }
        internal static Dictionary<short, ItemUsedOnBlockHandler> ItemUsedOnBlockHandlers { get; set; }
        
        static Item()
        {
            ItemNames = new Dictionary<short, string>();
            ToolTypes = new Dictionary<short, ToolType>();
            ItemMaterials = new Dictionary<short, ItemMaterial>();
            ItemUsedOnBlockHandlers = new Dictionary<short, ItemUsedOnBlockHandler>();
            ToolEfficiencyHandlers = new Dictionary<short, ToolEfficiencyHandler>();
            ReflectItems(typeof(Item).Assembly);
        }

        public static void ReflectItems(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(Item).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    LoadItem((Item)Activator.CreateInstance(type));
                }
            }
        }

        public static void LoadItem<T>() where T : Item
        {
            LoadItem(default(T));
        }

        internal static void LoadItem(Item item)
        {
            ItemNames[item.ItemId] = item.Name;
        }
        
        internal static void OnItemUsedOnBlock(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item)
        {
            if (ItemUsedOnBlockHandlers.ContainsKey(item.ItemId))
                ItemUsedOnBlockHandlers[item.ItemId](world, coordinates, face, cursor, item);
            else
            {
                // TEMPORARY
                if (item.ItemId <= 0x100)
                    Block.DefaultUsedOnBlockHandler(world, coordinates, face, cursor, item);
            }
        }
        
        public static ToolType? GetToolType(short itemId)
        {
            if (ToolTypes.ContainsKey(itemId))
                return ToolTypes[itemId];
            return null;
        }
        
        public static ItemMaterial? GetItemMaterial(short itemId)
        {
            if (ItemMaterials.ContainsKey(itemId))
                return ItemMaterials[itemId];
            return null;
        }
        
        public static bool IsEfficient(short itemId, short blockId)
        {
            if (ToolEfficiencyHandlers.ContainsKey(itemId))
                return ToolEfficiencyHandlers[itemId](blockId);
            return false;
        }
        
        public static bool Damage(ref ItemInfo item, short damage)
        {
            var toolType = GetToolType(item.ItemId);
            var material = GetItemMaterial(item.ItemId);
            if (toolType == null || toolType.Value == ToolType.Other || material == null)
                return false;
            item.Metadata += damage;
            return item.Metadata >= GetIdealNumberOfUses(material.Value);
        }
        
        public static short GetIdealNumberOfUses(ItemMaterial material)
        {
            switch (material)
            {
                case ItemMaterial.Wood:
                    return 60;
                case ItemMaterial.Stone:
                    return 132;
                case ItemMaterial.Iron:
                    return 251;
                case ItemMaterial.Gold:
                    return 33;
                case ItemMaterial.Diamond:
                    return 1562;
            }
            return short.MaxValue;
        }
        
        public string Name { get; set; }
        public abstract short ItemId { get; }
        
        protected Item(string name, ItemMaterial? material = null, ToolType? toolType = null)
        {
            Name = name;
            if (material != null)
                ItemMaterials[ItemId] = material.Value;
            if (toolType != null)
                ToolTypes[ItemId] = toolType.Value;
        }
        
        protected void SetItemUsedOnBlockHandler(ItemUsedOnBlockHandler handler)
        {
            ItemUsedOnBlockHandlers[ItemId] = handler;
        }
        
        protected void SetToolEfficiencyHandler(ToolEfficiencyHandler handler)
        {
            ToolEfficiencyHandlers[ItemId] = handler;
        }
    }
}