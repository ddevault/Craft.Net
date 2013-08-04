using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Common;

namespace Craft.Net.Server.Channels
{
    public class ServerPingChannel : PluginChannel
    {
        public event EventHandler<ServerPingEventArgs> ServerPingReceived;

        public override string Channel
        {
            get { return "MC|PingHost"; }
        }

        public override void MessageRecieved(RemoteClient client, byte[] data)
        {
            if (ServerPingReceived != null)
            {
                var stream = new MinecraftStream(new MemoryStream(data));
                var eventArgs = new ServerPingEventArgs(
                    stream.ReadInt32(),
                    stream.ReadString(),
                    stream.ReadInt32());
                ServerPingReceived(this, eventArgs);
            }
        }

        public override void ChannelRegistered(MinecraftServer server)
        {
        }

        public class ServerPingEventArgs : EventArgs
        {
            public int ProtocolVersion { get; set; }
            public string Hostname { get; set; }
            public int Port { get; set; }

            public ServerPingEventArgs(int protocolVersion, string hostname, int port)
            {
                ProtocolVersion = protocolVersion;
                Hostname = hostname;
                Port = port;
            }
        }
    }
}