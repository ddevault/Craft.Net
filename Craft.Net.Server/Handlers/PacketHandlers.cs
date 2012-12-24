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

            MinecraftServer.RegisterPacketHandler(PlayerDiggingPacket.PacketId, InteractionHandlers.PlayerDigging);
            MinecraftServer.RegisterPacketHandler(RightClickPacket.PacketId, InteractionHandlers.RightClick);
            MinecraftServer.RegisterPacketHandler(UseEntityPacket.PacketId, InteractionHandlers.UseEntity);

            MinecraftServer.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
            MinecraftServer.RegisterPacketHandler(PluginMessagePacket.PacketId, PluginMessage);
            MinecraftServer.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatMessage);
            MinecraftServer.RegisterPacketHandler(KeepAlivePacket.PacketId, KeepAlive);
        }

        public static void ServerListPing(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
        }

        public static void PluginMessage(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PluginMessagePacket)_packet;
            if (server.PluginChannels.ContainsKey(packet.Channel))
                server.PluginChannels[packet.Channel].MessageRecieved(client, packet.Data);
        }

        public static void ChatMessage(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            LogProvider.Log("<" + client.Username + "> " + packet.Message, LogImportance.Medium);
            var args = new ChatMessageEventArgs(client, packet.Message);
            server.OnChatMessage(args);
            if (!args.Handled)
                server.SendChat("<" + client.Username + "> " + packet.Message);
        }

        public static void KeepAlive(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (KeepAlivePacket)_packet;
            // TODO: Confirm value validity
            client.LastKeepAlive = DateTime.Now;
            client.Ping = (short)(client.LastKeepAlive - client.LastKeepAliveSent).TotalMilliseconds;
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
