using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class ServerListPingPacket : Packet
    {
        public override byte PacketId
        {
            get { return 0xFE; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            // This packet has no body
            return 1;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
            server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public string GetPingValue(MinecraftServer server)
        {
            return server.MotD + "§" +
                   server.Clients.Count(c => c.IsLoggedIn) + "§" +
                   server.MaxPlayers;
        }
    }
}