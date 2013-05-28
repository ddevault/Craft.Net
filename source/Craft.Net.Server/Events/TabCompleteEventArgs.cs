using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Events
{
    public class TabCompleteEventArgs : EventArgs
    {
        public TabCompleteEventArgs(string text, MinecraftClient client)
        {
            Text = text;
            Handled = false;
            Client = client;
        }

        public string Text { get; set; }
        public bool Handled { get; set; }
        public MinecraftClient Client { get; set; }
    }
}
