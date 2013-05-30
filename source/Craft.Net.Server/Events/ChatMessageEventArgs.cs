using System;

namespace Craft.Net.Server.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public bool Handled;
        public RemoteClient Origin;
        public string RawMessage;

        public ChatMessageEventArgs(RemoteClient origin, string rawMessage)
        {
            this.RawMessage = rawMessage;
            this.Origin = origin;
            Handled = false;
        }
    }
}