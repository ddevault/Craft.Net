using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Events;

namespace Craft.Net.Data
{
    public abstract class Window
    {
        public abstract WindowArea[] WindowAreas { get; protected set; }

        public event EventHandler<WindowChangeEventArgs> WindowChange;

        /// <summary>
        /// Called when an item is "shift+clicked" to move it from
        /// one area to another.
        /// </summary>
        public virtual void MoveToAlternateArea(int index)
        {
            int fromIndex = GetAreaIndex(index);
            var from = GetArea(ref index);
            var to = GetLinkedArea(fromIndex);
            var slot = from[index];
            int emptyIndex = -1;
            for (int i = 0; i < to.Length; i++)
            {
                if (to[i].Empty && emptyIndex == -1)
                    emptyIndex = i;
                else if (to[i].Id == slot.Id &&
                    to[i].Metadata == slot.Metadata &&
                    to[i].Count < slot.Item.MaximumStack)
                {
                    // Merging takes precedence over empty slots
                    emptyIndex = -1;
                    from[index] = new Slot();
                    if (to[i].Count + slot.Count > slot.Item.MaximumStack)
                    {
                        slot.Count -= (byte)(slot.Item.MaximumStack - to[i].Count);
                        to[i].Count = slot.Item.MaximumStack;
                        continue;
                    }
                    to[i].Count += slot.Count;
                    break;
                }
            }
            if (emptyIndex != -1)
            {
                from[index] = new Slot();
                to[emptyIndex] = slot;
            }
        }

        /// <summary>
        /// When shift-clicking items between areas, this method is used
        /// to determine which area links to which.
        /// </summary>
        /// <param name="index">The index of the area the item is coming from</param>
        /// <returns>The area to place the item into</returns>
        protected abstract WindowArea GetLinkedArea(int index);

        /// <summary>
        /// Gets the window area to handle this index and adjust index accordingly
        /// </summary>
        protected WindowArea GetArea(ref int index)
        {
            foreach (var area in WindowAreas)
            {
                if (area.StartIndex <= index && area.StartIndex + area.Length > index)
                {
                    index = index - area.StartIndex;
                    return area;
                }
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Gets the index of the appropriate area from the WindowAreas array.
        /// </summary>
        protected int GetAreaIndex(int index)
        {
            for (int i = 0; i < WindowAreas.Length; i++)
            {
                var area = WindowAreas[i];
                if (index >= area.StartIndex && index < area.StartIndex + area.Length)
                    return i;
            }
            throw new IndexOutOfRangeException();
        }

        public int Length
        {
            get 
            {
                return WindowAreas.Sum(a => a.Length);
            }
        }

        public Slot[] GetSlots()
        {
            int length = WindowAreas.Sum(area => area.Length);
            var slots = new Slot[length];
            foreach (var windowArea in WindowAreas)
                Array.Copy(windowArea.Items, 0, slots, windowArea.StartIndex, windowArea.Length);
            return slots;
        }

        public virtual Slot this[int index]
        {
            get
            {
                foreach (var area in WindowAreas)
                {
                    if (index >= area.StartIndex && index < area.StartIndex + area.Length)
                        return area[index - area.StartIndex];
                }
                throw new IndexOutOfRangeException();
            }
            set
            {
                foreach (var area in WindowAreas)
                {
                    if (index >= area.StartIndex && index < area.StartIndex + area.Length)
                    {
                        var eventArgs = new WindowChangeEventArgs(index, value);
                        if (WindowChange != null)
                            WindowChange(this, eventArgs);
                        if (!eventArgs.Handled)
                            area[index - area.StartIndex] = value;
                        return;
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }
    }
}
