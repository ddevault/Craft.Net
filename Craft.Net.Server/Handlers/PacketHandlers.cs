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
            MinecraftServer.RegisterPacketHandler(EncryptionKeyResponsePacket.PacketId, LoginHandlers.EncryptionKeyResponse);
            MinecraftServer.RegisterPacketHandler(ClientStatusPacket.PacketId, LoginHandlers.ClientStatus);
            MinecraftServer.RegisterPacketHandler(ClientSettingsPacket.PacketId, LoginHandlers.ClientSettings);

            MinecraftServer.RegisterPacketHandler(PlayerPacket.PacketId, PlayerMovementHandlers.Player);
            MinecraftServer.RegisterPacketHandler(PlayerPositionPacket.PacketId, PlayerMovementHandlers.PlayerPosition);
            MinecraftServer.RegisterPacketHandler(PlayerLookPacket.PacketId, PlayerMovementHandlers.PlayerLook);
            MinecraftServer.RegisterPacketHandler(PlayerPositionAndLookPacket.PacketId, PlayerMovementHandlers.PlayerPositionAndLook);
            MinecraftServer.RegisterPacketHandler(AnimationPacket.PacketId, PlayerMovementHandlers.Animation);

            MinecraftServer.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
        }

        public static void ServerListPing(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
        }

        public static void PluginMessage(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var message = (PluginMessagePacket)packet;
            if (server.PluginChannels.ContainsKey(message.Channel))
                server.PluginChannels[message.Channel].MessageRecieved(client, message.Data);
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
