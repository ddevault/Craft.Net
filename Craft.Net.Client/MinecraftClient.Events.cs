using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Client.Events;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public event EventHandler LoggedIn;
        protected internal virtual void OnLoggedIn()
        {
            if (LoggedIn != null) LoggedIn(this, null);
        }

        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null) ChatMessage(this, e);
        }

        public event EventHandler<EntitySpawnEventArgs> InitialSpawn;
        protected internal virtual void OnInitialSpawn(EntitySpawnEventArgs e)
        {
            if (InitialSpawn != null) InitialSpawn(this, e);
        }

        public event EventHandler<DisconnectEventArgs> Disconnected;
        protected internal virtual void OnDisconnected(DisconnectEventArgs e)
        {
            if (Disconnected != null) Disconnected(this, e);
        }
    }
}
