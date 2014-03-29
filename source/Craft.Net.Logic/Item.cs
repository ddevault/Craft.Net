using System;
using System.Reflection;
using System.Collections.Generic;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace Craft.Net.Logic
{
    public abstract class Item
    {
        private static Dictionary<short, string> ItemNames { get; set; }
        private static Dictionary<short, ToolType> ToolTypes { get; set; }
        private static Dictionary<short, ItemMaterial> ItemMaterials { get; set; }
        public delegate void ItemUsedOnBlockHandler(World world, Coordinates3D coordinates, BlockFace face, Coordinates3D cursor, ItemInfo item);
        internal static Dictionary<short, ItemUsedOnBlockHandler> ItemUsedOnBlockHandlers { get; set; }
        
        static Item()
        {
            ItemNames = new Dictionary<short, string>();
            ToolTypes = new Dictionary<short, ToolType>();
            ItemMaterials = new Dictionary<short, ItemMaterial>();
            ItemUsedOnBlockHandlers = new Dictionary<short, ItemUsedOnBlockHandler>();
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
            if (item.ToolType != null)
                ToolTypes[item.ItemId] = item.ToolType.Value;
            if (item.Material != null)
                ItemMaterials[item.ItemId] = item.Material.Value;
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
        
        public string Name { get; set; }
        public abstract short ItemId { get; }
        public ToolType? ToolType { get; set; }
        public ItemMaterial? Material { get; set; }
        
        protected Item(string name, ItemMaterial? material = null, ToolType? toolType = null)
        {
            Name = name;
            Material = material;
            ToolType = toolType;
        }
        
        protected void SetItemUsedOnBlockHandler(ItemUsedOnBlockHandler handler)
        {
            ItemUsedOnBlockHandlers[ItemId] = handler;
        }
    }
}