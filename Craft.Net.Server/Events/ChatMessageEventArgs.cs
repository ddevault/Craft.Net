using System;

namespace Craft.Net.Server.Events
{
    public class ChatMessageEventArgs : EventArgs
    {
        public string RawMessage;
        public MinecraftClient Origin;
        public bool Handled;

        public ChatMessageEventArgs(MinecraftClient Origin, string RawMessage)
        {
            this.RawMessage = RawMessage;
            this.Origin = Origin;
            this.Handled = false;
        }
    }
}

