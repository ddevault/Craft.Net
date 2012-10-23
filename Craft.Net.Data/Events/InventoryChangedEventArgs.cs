using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Data.Events
{
    public class InventoryChangedEventArgs : EventArgs
    {
        public Slot NewValue, OldValue;
        public short Index;
    }
}
