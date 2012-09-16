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

        public Slot this[int index]
        {
            get { return Items[index]; }
            set { Items[index] = value; }
        }
    }
}
