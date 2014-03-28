using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Networking;

namespace Craft.Net.Client.Events
{

    public class ChatMessageEventArgs : EventArgs
    {
        public ChatMessage Message { get; set; }


        public ChatMessageEventArgs(ChatMessage Message)
        {
            this.Message = Message;
        }
    }
}