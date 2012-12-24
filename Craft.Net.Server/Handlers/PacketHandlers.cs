using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Server.Events;

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

            MinecraftServer.RegisterPacketHandler(CreativeInventoryActionPacket.PacketId, InventoryHandlers.CreativeInventoryAction);
            MinecraftServer.RegisterPacketHandler(ClickWindowPacket.PacketId, InventoryHandlers.ClickWindow);
            MinecraftServer.RegisterPacketHandler(CloseWindowPacket.PacketId, InventoryHandlers.CloseWindow);
            MinecraftServer.RegisterPacketHandler(HeldItemChangePacket.PacketId, InventoryHandlers.HeldItemChange);

            MinecraftServer.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
            MinecraftServer.RegisterPacketHandler(PluginMessagePacket.PacketId, PluginMessage);
            MinecraftServer.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatMessage);
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

        public static void ChatMessage(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var message = (ChatMessagePacket)packet;
            LogProvider.Log("<" + client.Username + "> " + message.Message, LogImportance.Medium);
            var args = new ChatMessageEventArgs(client, message.Message);
            server.OnChatMessage(args);
            if (!args.Handled)
                server.SendChat("<" + client.Username + "> " + message.Message);
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
