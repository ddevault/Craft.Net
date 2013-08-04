using System;
using Craft.Net.Networking;

namespace Craft.Net.Server
{
    /// <summary>
    /// A channel for mods to communicate with clients.
    /// </summary>
    public abstract class PluginChannel
    {
        #pragma warning disable
        internal MinecraftServer Server;
        #pragma warning restore

        /// <summary>
        /// This channel name.
        /// </summary>
        public abstract string Channel { get; }

        /// <summary>
        /// Run when a plugin message is recieved.
        /// </summary>
        public abstract void MessageRecieved(RemoteClient client, byte[] data);

        /// <summary>
        /// Run when the channel is successfully registered.
        /// </summary>
        public abstract void ChannelRegistered(MinecraftServer server);

        /// <summary>
        /// Sends a channel message to the given client.
        /// </summary>
        public void SendMessage(byte[] data, RemoteClient client)
        {
            if (data.Length > short.MaxValue)
                throw new ArgumentOutOfRangeException("Maximum plugin message length is " + short.MaxValue);
            client.SendPacket(new PluginMessagePacket(Channel, data));
        }
    }
}