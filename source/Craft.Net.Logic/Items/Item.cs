using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Anvil;
using System.Reflection;
using Craft.Net.Logic.Blocks;

namespace Craft.Net.Logic.Items
{
    public struct ItemDescriptor
    {
        public ItemDescriptor(short id)
        {
            Id = id;
            Metadata = 0;
        }

        public ItemDescriptor(short id, short metadata)
        {
            Id = id;
            Metadata = metadata;
        }

        public short Id;
        public short Metadata;
    }

    public class ItemLogicDescriptor
    {
        public ItemLogicDescriptor(Type itemType)
        {
            // Default handlers
            ItemType = itemType;
            ItemUsed = Item.DefaultItemUsedHandler;
            ItemUsedOnBlock = Item.DefaultItemUsedOnBlockHandler;
            MaximumStackSize = 64;
        }

        public Type ItemType;

        public Item.ItemUsedDelegate ItemUsed;
        public Item.ItemUsedOnBlockDelegate ItemUsedOnBlock;

        public int MaximumStackSize;
    }

    public static class Item
    {
        public delegate void ItemInitializerDelegate(ItemLogicDescriptor descriptor);
        public delegate void ItemUsedDelegate(ItemDescriptor item); // TODO: Entities
        public delegate void ItemUsedOnBlockDelegate(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition);
        public delegate bool IsEfficientDelegate(ItemDescriptor item, BlockDescriptor block);

        private static Dictionary<short, ItemLogicDescriptor> ItemLogicDescriptors { get; set; }

        static Item()
        {
            LoadItems();
        }

        private static void LoadItems()
        {
            if (ItemLogicDescriptors != null)
                return;
            ItemLogicDescriptors = new Dictionary<short, ItemLogicDescriptor>();
            // Loads all item classes in Craft.Net.Logic
            var types = typeof(Item).Assembly.GetTypes().Where(t => t.GetCustomAttributes<ItemAttribute>().Any()).ToArray();
            LoadTypes(types);
        }

        public static void LoadTypes(Type[] types)
        {
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttributes<ItemAttribute>().First();
                var initializerType = type;
                if (attribute.InitializerType != null)
                    initializerType = attribute.InitializerType;
                var method = initializerType.GetMethods().FirstOrDefault(m => m.Name == attribute.Initializer
                    && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(ItemLogicDescriptor) && !m.IsGenericMethod);
                var descriptor = new ItemLogicDescriptor(type);
                if (method != null)
                    method.Invoke(null, new object[] { descriptor });
                ItemLogicDescriptors[attribute.ItemId] = descriptor;
            }
        }

        public static void OnItemUsed(ItemDescriptor item)
        {
            GetLogicDescriptor(item).ItemUsed(item);
        }

        public static void OnItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            GetLogicDescriptor(item).ItemUsedOnBlock(item, world, clickedBlock, clickedSide, cursorPosition);
        }

        public static int GetMaximumStackSize(ItemDescriptor item)
        {
            return GetLogicDescriptor(item).MaximumStackSize;
        }

        public static ItemLogicDescriptor GetLogicDescriptor(ItemDescriptor item)
        {
            if (!ItemLogicDescriptors.ContainsKey(item.Id))
                throw new KeyNotFoundException("The given item does not exist.");
            return ItemLogicDescriptors[item.Id];
        }

        #region Default handlers

        internal static void DefaultItemUsedHandler(ItemDescriptor item)
        {
        }

        internal static void DefaultItemUsedOnBlockHandler(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
        }

        #endregion
    }
}
