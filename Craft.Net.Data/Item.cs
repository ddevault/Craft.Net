using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;

namespace Craft.Net.Data
{
    /// <summary>
    /// Represents an item in Minecraft.
    /// </summary>
    public abstract class Item : IComparable
    {
        static Item()
        {
            // The items should always be kept in order
            Items.Sort();
        }

        /// <summary>
        /// This item's ID.
        /// </summary>
        public abstract ushort Id { get; }
        /// <summary>
        /// The metadata or durability of this item.
        /// </summary>
        public abstract ushort Data { get; set; }

        public virtual void OnItemUsed(Vector3 clickedBlock, Vector3 clickedSide, Vector3 cursorPosition, World world, Entity usedBy)
        {
        }

        #region Items Conversion

        public static implicit operator ushort(Item i)
        {
            return i.Id;
        }

        public static implicit operator Item(ushort u)
        {
            // Binary search through items
            int index = GetItemIndex(u);
            if (index == -1)
                return null;
            return Items[index];
        }

        private static int GetItemIndex(ushort item)
        {
            if (item > Items[Items.Count - 1].Id)
                return -1;
            int a = 0, b = Items.Count;
            while (b >= a)
            {
                int c = (a + b) / 2;
                if (item > Items[c].Id)
                    a = c + 1;
                else if (item < Items[c].Id)
                    b = c - 1;
                else
                    return c;
            }
            return -1;
        }

        /// <summary>
        /// Use this method to override the implementation of
        /// default items, or to add your own.
        /// </summary>
        public static void SetItemClass(Item item)
        {
            int index = GetItemIndex(item.Id);
            if (index != -1)
                Items[index] = item;
            else
            {
                Items.Add(item);
                Items.Sort();
            }
        }

        private static List<Item> Items = new List<Item>(new Item[]
        {
            new AirBlock(),
            new BedrockBlock(),
            new CoalOreBlock(),
            new CobblestoneBlock(),
            new DirtBlock(),
            new GrassBlock(),
            new GoldBlock(),
            new GoldOreBlock(),
            new GrassBlock(),
            new GravelBlock(),
            new IronOreBlock(),
            new LavaFlowingBlock(),
            new LavaStillBlock(),
            new LeavesBlock(),
            new LogBlock(),
            new SandBlock(),
            new SaplingBlock(),
            new SpongeBlock(),
            new StoneBlock(),
            new WaterFlowingBlock(),
            new WaterStillBlock(),
            new WoodenPlanksBlock(),
            new WoolBlock()
        });

        #endregion

        #region IComparable

        public int CompareTo(object obj)
        {
            return this.Id - ((Item)obj).Id;
        }

        #endregion
    }
}
