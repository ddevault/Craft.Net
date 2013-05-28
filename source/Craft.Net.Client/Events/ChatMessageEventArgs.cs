using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public string RawMessage { get; set; }
        public string CleanMessage { get; set; }

        public ChatMessageEventArgs(string rawMessage, string cleanMessage)
        {
            // TODO: Parse vanilla chat
            RawMessage = rawMessage;
            CleanMessage = cleanMessage;
        }
    }
}
