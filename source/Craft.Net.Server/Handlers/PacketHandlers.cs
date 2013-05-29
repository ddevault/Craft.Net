using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Handlers
{
    internal static class PacketHandlers
    {
        public static void RegisterHandlers(MinecraftServer server)
        {
            server.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
        }

        public static void ServerListPing(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
        }

        private static string GetPingValue(MinecraftServer server)
        {
            return "§1\0" + PacketReader.ProtocolVersion + "\0" +
                PacketReader.FriendlyVersion + "\0" +
                server.Settings.MotD + "\0" + server.Clients.Count(c => c.IsLoggedIn) +
                "\0" + server.Settings.MaxPlayers;
        }
    }
}
