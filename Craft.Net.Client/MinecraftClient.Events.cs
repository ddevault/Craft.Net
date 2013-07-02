using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Client.Events;

namespace Craft.Net.Client
{
    public partial class MinecraftClient
    {
        public event EventHandler<PacketEventArgs> PacketRecieved;
        protected internal virtual void OnPacketRecieved(PacketEventArgs e)
        {
            if (PacketRecieved != null) PacketRecieved(this, e);
        }

        public event EventHandler<PacketEventArgs> PacketSent;
        protected internal virtual void OnPacketSent(PacketEventArgs e)
        {
            if (PacketSent != null) PacketSent(this, e);
        }

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

        public event EventHandler PlayerDied;
        protected internal virtual void OnPlayerDied()
        {
            if (PlayerDied != null) PlayerDied(this, null);
        }

        public event EventHandler<HealthAndFoodEventArgs> HealthOrFoodChanged;
        protected internal virtual void OnHealthOrFoodChanged(HealthAndFoodEventArgs e)
        {
            if (HealthOrFoodChanged != null) HealthOrFoodChanged(this, e);
        }

        public event EventHandler WorldInitialized;
        protected internal virtual void OnWorldInitialized()
        {
            if (WorldInitialized != null) WorldInitialized(this, null);
        }

        public event EventHandler<ChunkRecievedEventArgs> ChunkRecieved;
        protected internal virtual void OnChunkRecieved(ChunkRecievedEventArgs e)
        {
            if (ChunkRecieved != null) ChunkRecieved(this, e);
        }

        public event EventHandler<SignUpdateReceivedEventArgs> SignUpdateReceived;
        protected internal virtual void OnSignUpdateReceived(SignUpdateReceivedEventArgs e)
        {
            if (SignUpdateReceived != null) SignUpdateReceived(this, e);
        }
    }
}
