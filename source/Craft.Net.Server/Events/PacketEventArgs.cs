using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Events
{
    public class PacketEventArgs : EventArgs
    {
        public PacketEventArgs(IPacket packet, MinecraftClient client, MinecraftServer server, PacketContext context)
        {
            Packet = packet;
            Client = client;
            Server = server;
        }

        public IPacket Packet { get; set; }
        public PacketContext Context { get; set; }
        public MinecraftClient Client { get; set; }
        public MinecraftServer Server { get; set; }
    }
}
