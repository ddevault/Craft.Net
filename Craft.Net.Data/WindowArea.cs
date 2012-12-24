using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Events;

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
                Items[i] = Slot.EmptySlot;
        }

        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Slot[] Items { get; set; }
        public event EventHandler<WindowChangeEventArgs> WindowChange;

        public virtual Slot this[int index]
        {
            get { return Items[index]; }
            set
            {
                if (IsValid(value, index))
                    Items[index] = value;
                OnWindowChange(new WindowChangeEventArgs(index, value));
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
                         this[i].Count < slot.AsItem().MaximumStack)
                {
                    // Merging takes precedence over empty slots
                    emptyIndex = -1;
                    if (from != null)
                        from[index] = Slot.EmptySlot;
                    if (this[i].Count + slot.Count > slot.AsItem().MaximumStack)
                    {
                        slot = new Slot(slot.Id, (sbyte)(slot.Count - (slot.AsItem().MaximumStack - this[i].Count)),
                            slot.Metadata, slot.Nbt);
                        this[i] = new Slot(slot.Id, (sbyte)slot.AsItem().MaximumStack, slot.Metadata, slot.Nbt);
                        continue;
                    }
                    this[i] = new Slot(slot.Id, (sbyte)(this[i].Count + slot.Count));
                    return i;
                }
            }
            if (emptyIndex != -1)
            {
                if (from != null)
                    from[index] = Slot.EmptySlot;
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

        protected internal virtual void OnWindowChange(WindowChangeEventArgs e)
        {
            if (WindowChange != null)
                WindowChange(this, e);
        }
    }
}
