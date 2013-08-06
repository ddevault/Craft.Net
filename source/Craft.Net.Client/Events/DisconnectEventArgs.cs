using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Events
{
    public class DisconnectEventArgs : EventArgs
    {
        public string Reason { get; set; }

        public DisconnectEventArgs(string reason)
        {
            Reason = reason;
        }
    }
}