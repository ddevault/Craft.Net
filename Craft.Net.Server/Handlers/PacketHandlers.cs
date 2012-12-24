using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Handlers
{
    internal static class PacketHandlers
    {
        /// <summary>
        /// Registers all built-in packet handlers.
        /// </summary>
        public static void RegisterHandlers()
        {
            MinecraftServer.RegisterPacketHandler(HandshakePacket.PacketId, LoginHandlers.Handshake);

            MinecraftServer.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
        }

        internal static void ServerListPing(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
        }

        private static string GetPingValue(MinecraftServer server)
        {
            return "§1\0" + MinecraftServer.ProtocolVersion + "\0" +
                MinecraftServer.TargetClientVersion + "\0" +
                server.Settings.MotD + "\0" + server.Clients.Count(c => c.IsLoggedIn) +
                "\0" + server.Settings.MaxPlayers;
        }
    }
}
