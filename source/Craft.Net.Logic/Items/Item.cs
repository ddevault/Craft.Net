using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Anvil;
using System.Reflection;

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
        public ItemLogicDescriptor()
        {
            // Default handlers
            ItemUsed = Item.DefaultItemUsed;
            ItemUsedOnBlock = Item.DefaultItemUsedOnBlock;
        }

        public Item.ItemUsedDelegate ItemUsed;
        public Item.ItemUsedOnBlockDelegate ItemUsedOnBlock;
    }

    public static class Item
    {
        public delegate void ItemInitializerDelegate(ItemLogicDescriptor descriptor);
        public delegate void ItemUsedDelegate(ItemDescriptor item); // TODO: Entities
        public delegate void ItemUsedOnBlockDelegate(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition);

        private static Dictionary<short, ItemLogicDescriptor> ItemLogicDescriptors { get; set; }

        static Item()
        {
            LoadItems();
        }

        public static void LoadItems()
        {
            if (ItemLogicDescriptors != null)
                return;
            ItemLogicDescriptors = new Dictionary<short, ItemLogicDescriptor>();
            // Loads all item classes in Craft.Net.Logic
            var types = typeof(Item).Assembly.GetTypes().Where(t => t.GetCustomAttributes<MinecraftItemAttribute>().Any()).ToArray();
            LoadTypes(types);
        }

        public static void LoadTypes(Type[] types)
        {
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttributes<MinecraftItemAttribute>().First();
                var method = type.GetMethods().FirstOrDefault(m => m.Name == attribute.Initializer
                    && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(ItemLogicDescriptor) && !m.IsGenericMethod);
                var descriptor = new ItemLogicDescriptor();
                method.Invoke(null, new object[] { descriptor });
                ItemLogicDescriptors[attribute.ItemId] = descriptor;
            }
        }

        public static void OnItemUsed(ItemDescriptor item)
        {
            if (!ItemLogicDescriptors.ContainsKey(item.Id))
                throw new KeyNotFoundException("The given item does not exist.");
            ItemLogicDescriptors[item.Id].ItemUsed(item);
        }

        public static void OnItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            if (!ItemLogicDescriptors.ContainsKey(item.Id))
                throw new KeyNotFoundException("The given item does not exist.");
            ItemLogicDescriptors[item.Id].ItemUsedOnBlock(item, world, clickedBlock, clickedSide, cursorPosition);
        }

        #region Default handlers

        internal static void DefaultItemUsed(ItemDescriptor item)
        {
        }

        internal static void DefaultItemUsedOnBlock(ItemDescriptor item, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
        }

        #endregion
    }
}
