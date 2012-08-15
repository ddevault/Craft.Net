using System;
using Craft.Net.Server.Packets;

namespace Craft.Net.Server
{
    public abstract class PluginChannel
    {
        internal MinecraftServer Server;

        public abstract string Channel { get; }

        public virtual void MessageRecieved(byte[] data)
        {
        }

        public virtual void ChannelRegistered(MinecraftServer server)
        {
        }

        public void SendMessage(byte[] data, MinecraftClient client)
        {
            if (data.Length > short.MaxValue)
                throw new ArgumentOutOfRangeException("Maximum plugin message length is " + short.MaxValue);
            client.SendPacket(new PluginMessagePacket(Channel, data));
        }
    }

    public class PluginChannelMessageEvengArgs : EventArgs
    {
        public byte Data { get; set; }
    }
}