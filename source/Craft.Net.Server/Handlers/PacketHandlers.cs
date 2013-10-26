using Craft.Net.Networking;
using Craft.Net.Server.Events;
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
            server.RegisterPacketHandler(HandshakePacket.PacketId, LoginHandlers.Handshake);
            server.RegisterPacketHandler(EncryptionKeyResponsePacket.PacketId, LoginHandlers.EncryptionKeyResponse);
            server.RegisterPacketHandler(ClientStatusPacket.PacketId, LoginHandlers.ClientStatus);
            server.RegisterPacketHandler(ClientSettingsPacket.PacketId, LoginHandlers.ClientSettings);

            server.RegisterPacketHandler(PlayerPacket.PacketId, PlayerMovementHandlers.Player);
            server.RegisterPacketHandler(PlayerPositionPacket.PacketId, PlayerMovementHandlers.PlayerPosition);
            server.RegisterPacketHandler(PlayerLookPacket.PacketId, PlayerMovementHandlers.PlayerLook);
            server.RegisterPacketHandler(PlayerPositionAndLookPacket.PacketId, PlayerMovementHandlers.PlayerPositionAndLook);
            server.RegisterPacketHandler(AnimationPacket.PacketId, PlayerMovementHandlers.Animation);
            server.RegisterPacketHandler(EntityActionPacket.PacketId, PlayerMovementHandlers.EntityAction);
            server.RegisterPacketHandler(PlayerAbilitiesPacket.PacketId, PlayerMovementHandlers.PlayerAbilities);

            server.RegisterPacketHandler(CreativeInventoryActionPacket.PacketId, InventoryHandlers.CreativeInventoryAction);
            server.RegisterPacketHandler(ClickWindowPacket.PacketId, InventoryHandlers.ClickWindow);
            server.RegisterPacketHandler(CloseWindowPacket.PacketId, InventoryHandlers.CloseWindow);
            server.RegisterPacketHandler(HeldItemChangePacket.PacketId, InventoryHandlers.HeldItemChange);

            server.RegisterPacketHandler(PlayerDiggingPacket.PacketId, InteractionHandlers.PlayerDigging);
            server.RegisterPacketHandler(RightClickPacket.PacketId, InteractionHandlers.RightClick);

            server.RegisterPacketHandler(ServerListPingPacket.PacketId, ServerListPing);
            server.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatMessage);
            server.RegisterPacketHandler(KeepAlivePacket.PacketId, KeepAlive);
        }

        public static void ServerListPing(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            client.SendPacket(new DisconnectPacket(GetPingValue(server)));
        }

        public static void ChatMessage(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            //LogProvider.Log("<" + client.Username + "> " + packet.Message, LogImportance.Medium);
            var args = new ChatMessageEventArgs(client, packet.Message);
            server.OnChatMessage(args);
            if (!args.Handled)
            {
                //var team = server.ScoreboardManager.GetPlayerTeam(client.Username);
                string chat;
                //if (team != null)
                //    chat = string.Format("<{0}{1}{2}> {3}", team.PlayerPrefix, client.Username, team.PlayerSuffix, packet.Message);
                //else
                    chat = string.Format("<{0}> {1}", client.Username, packet.Message);
                server.SendChat(chat);
            }
        }

        public static void KeepAlive(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            // TODO: Confirm value validity
            client.LastKeepAlive = DateTime.Now;
            client.Ping = (short)(client.LastKeepAlive - client.LastKeepAliveSent).TotalMilliseconds;
        }

        private static string GetPingValue(MinecraftServer server)
        {
            return "§1\0" + PacketHandler.ProtocolVersion + "\0" +
                PacketHandler.FriendlyVersion + "\0" +
                server.Settings.MotD + "\0" + server.Clients.Count(c => c.IsLoggedIn) +
                "\0" + server.Settings.MaxPlayers;
        }
    }
}
