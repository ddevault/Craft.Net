using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class ServerListPingPacket : Packet
    {
        public override byte PacketID
        {
            get { return 0xFE; }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            // This packet has no body
            return 1;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Client.SendPacket(new DisconnectPacket(GetPingValue(Server)));
            Server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        public string GetPingValue(MinecraftServer Server)
        {
            return Server.MotD + "§" +
                   Server.Clients.Where(c => c.IsLoggedIn).Count() + "§" +
                   Server.MaxPlayers;
        }
    }
}