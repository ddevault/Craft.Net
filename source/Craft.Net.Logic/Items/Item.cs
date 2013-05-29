using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Anvil;
using System.Reflection;

namespace Craft.Net.Logic.Items
{
    public static class Item
    {
        public class ItemDescriptor
        {
            public ItemDescriptor()
            {
                // Default handlers
                ItemUsed = Item.DefaultItemUsed;
                ItemUsedOnBlock = Item.DefaultItemUsedOnBlock;
            }

            public ItemUsedDelegate ItemUsed;
            public ItemUsedOnBlockDelegate ItemUsedOnBlock;
        }

        public delegate void ItemInitializerDelegate(ItemDescriptor descriptor);
        public delegate void ItemUsedDelegate(); // TODO: Entities
        public delegate void ItemUsedOnBlockDelegate(World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition);

        private static Dictionary<short, ItemDescriptor> ItemDescriptors { get; set; }

        static Item()
        {
            LoadItems();
        }

        public static void LoadItems()
        {
            if (ItemDescriptors != null)
                return;
            ItemDescriptors = new Dictionary<short, ItemDescriptor>();
            // Loads all item classes in Craft.Net.Logic
            var types = typeof(Item).Assembly.GetTypes().Where(t => t.GetCustomAttributes<MinecraftItemAttribute>().Any());
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttributes<MinecraftItemAttribute>().First();
                var method = type.GetMethods().FirstOrDefault(m => m.Name == attribute.Initializer 
                    && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(ItemDescriptor) && !m.IsGenericMethod);
                var descriptor = new ItemDescriptor();
                method.Invoke(null, new object[] { descriptor });
                ItemDescriptors[attribute.ItemId] = descriptor;
            }
        }

        public static void OnItemUsed(short itemId)
        {
            if (!ItemDescriptors.ContainsKey(itemId))
                throw new KeyNotFoundException("The given item does not exist.");
            ItemDescriptors[itemId].ItemUsed();
        }

        public static void OnItemUsedOnBlock(short itemId, World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
            if (!ItemDescriptors.ContainsKey(itemId))
                throw new KeyNotFoundException("The given item does not exist.");
            ItemDescriptors[itemId].ItemUsedOnBlock(world, clickedBlock, clickedSide, cursorPosition);
        }

        #region Default handlers

        private static void DefaultItemUsed()
        {
        }

        private static void DefaultItemUsedOnBlock(World world, Coordinates3D clickedBlock, Coordinates3D clickedSide, Coordinates2D cursorPosition)
        {
        }

        #endregion
    }
}
