using System;

namespace Craft.Net.Server.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public bool Handled;
        public MinecraftClient Origin;
        public string RawMessage;

        public ChatMessageEventArgs(MinecraftClient origin, string rawMessage)
        {
            this.RawMessage = rawMessage;
            this.Origin = origin;
            Handled = false;
        }
    }
}