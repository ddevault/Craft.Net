using System;

namespace Craft.Net.Server.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public bool Handled;
        public MinecraftClient Origin;
        public string RawMessage;

        public ChatMessageEventArgs(MinecraftClient Origin, string RawMessage)
        {
            this.RawMessage = RawMessage;
            this.Origin = Origin;
            Handled = false;
        }
    }
}