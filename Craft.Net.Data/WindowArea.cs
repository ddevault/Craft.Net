using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data
{
    public class WindowArea
    {
        public WindowArea(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
            Items = new Slot[Length];
            for (int i = 0; i < Items.Length; i++)
                Items[i] = new Slot();
        }

        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Slot[] Items { get; set; }

        public virtual Slot this[int index]
        {
            get { return Items[index]; }
            set
            {
                if (IsValid(value, index))
                    Items[index] = value;
            }
        }

        protected internal virtual int MoveOrMergeItem(int index, Slot slot, WindowArea from)
        {
            int emptyIndex = -1;
            for (int i = 0; i < Length; i++)
            {
                if (this[i].Empty && emptyIndex == -1)
                    emptyIndex = i;
                else if (this[i].Id == slot.Id &&
                         this[i].Metadata == slot.Metadata &&
                         this[i].Count < slot.Item.MaximumStack)
                {
                    // Merging takes precedence over empty slots
                    emptyIndex = -1;
                    from[index] = new Slot();
                    if (this[i].Count + slot.Count > slot.Item.MaximumStack)
                    {
                        slot.Count -= (byte)(slot.Item.MaximumStack - this[i].Count);
                        this[i].Count = slot.Item.MaximumStack;
                        continue;
                    }
                    this[i].Count += slot.Count;
                    break;
                }
            }
            if (emptyIndex != -1)
            {
                from[index] = new Slot();
                this[emptyIndex] = slot;
            }
            return emptyIndex;
        }

        /// <summary>
        /// Returns true if the specified slot is valid to
        /// be placed in this index.
        /// </summary>
        protected virtual bool IsValid(Slot slot, int index)
        {
            return true;
        }

        public void CopyTo(WindowArea area)
        {
            for (int i = 0; i < area.Length && i < Length; i++)
                area[i] = this[i];
        }
    }
}
