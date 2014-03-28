using System;
using Craft.Net.Networking;

namespace Craft.Net.Server.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public bool Handled;
        public RemoteClient Origin;
        public ChatMessage Message;

        public ChatMessageEventArgs(RemoteClient origin, ChatMessage rawMessage)
        {
            this.Message = rawMessage;
            this.Origin = origin;
            Handled = false;
        }
    }
}