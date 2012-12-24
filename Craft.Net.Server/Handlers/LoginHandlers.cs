using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Handlers
{
    /// <summary>
    /// Packet handlers for login, handshake, encryption, etc
    /// </summary>
    internal static class LoginHandlers
    {
        public static void Handshake(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var handshake = (HandshakePacket)packet;
            if (handshake.ProtocolVersion < PacketReader.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated client!"));
                return;
            }
            if (handshake.ProtocolVersion > PacketReader.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated server!"));
                return;
            }
            if (server.Clients.Any(c => c.Username == handshake.Username))
            {
                client.SendPacket(new DisconnectPacket(""));
                return;
            }
        }
    }
}
