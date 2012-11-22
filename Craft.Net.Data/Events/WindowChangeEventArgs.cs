using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Events
{
   public class WindowChangeEventArgs : EventArgs
   {
      public int SlotIndex { get; set; }
      public Slot Value { get; set; }
      public bool Handled { get; set; }

      public WindowChangeEventArgs(int slotIndex, Slot value)
      {
         SlotIndex = slotIndex;
         Value = value;
         Handled = false;
      }
   }
}