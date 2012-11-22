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

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
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
            // TODO: Support <1.4
            return "§1\0" + MinecraftServer.ProtocolVersion + "\0" +
                MinecraftServer.TargetClientVersion + "\0" +
                server.Settings.MotD + "\0" + server.Clients.Count(c => c.IsLoggedIn) +
                "\0" + server.Settings.MaxPlayers;
            }
        }
}