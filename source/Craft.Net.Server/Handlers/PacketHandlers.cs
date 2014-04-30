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
            server.RegisterPacketHandler(typeof(HandshakePacket), LoginHandlers.Handshake);
            server.RegisterPacketHandler(typeof(LoginStartPacket), LoginHandlers.LoginStart);
            server.RegisterPacketHandler(typeof(EncryptionKeyResponsePacket), LoginHandlers.EncryptionKeyResponse);
            server.RegisterPacketHandler(typeof(ClientStatusPacket), LoginHandlers.ClientStatus);
            server.RegisterPacketHandler(typeof(ClientSettingsPacket), LoginHandlers.ClientSettings);

            server.RegisterPacketHandler(typeof(PlayerPacket), PlayerMovementHandlers.Player);
            server.RegisterPacketHandler(typeof(PlayerPositionPacket), PlayerMovementHandlers.PlayerPosition);
            server.RegisterPacketHandler(typeof(PlayerLookPacket), PlayerMovementHandlers.PlayerLook);
            server.RegisterPacketHandler(typeof(PlayerPositionAndLookPacket), PlayerMovementHandlers.PlayerPositionAndLook);
            server.RegisterPacketHandler(typeof(AnimationPacket), PlayerMovementHandlers.Animation);
            server.RegisterPacketHandler(typeof(EntityActionPacket), PlayerMovementHandlers.EntityAction);
            server.RegisterPacketHandler(typeof(PlayerAbilitiesPacket), PlayerMovementHandlers.PlayerAbilities);

            server.RegisterPacketHandler(typeof(CreativeInventoryActionPacket), InventoryHandlers.CreativeInventoryAction);
            server.RegisterPacketHandler(typeof(ClickWindowPacket), InventoryHandlers.ClickWindow);
            server.RegisterPacketHandler(typeof(CloseWindowPacket), InventoryHandlers.CloseWindow);
            server.RegisterPacketHandler(typeof(HeldItemPacket), InventoryHandlers.HeldItemChange);

            server.RegisterPacketHandler(typeof(PlayerBlockActionPacket), InteractionHandlers.PlayerDigging);
            server.RegisterPacketHandler(typeof(RightClickPacket), InteractionHandlers.RightClick);

            server.RegisterPacketHandler(typeof(StatusRequestPacket), StatusRequest);
            server.RegisterPacketHandler(typeof(StatusPingPacket), StatusPing);
            server.RegisterPacketHandler(typeof(ChatMessagePacket), ChatMessage);
            server.RegisterPacketHandler(typeof(KeepAlivePacket), KeepAlive);
        }

        public static void StatusRequest(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            client.SendPacket(new StatusResponsePacket(GetServerStatus(server)));
        }

        public static void StatusPing(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            client.SendPacket(_packet);
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
                ChatMessage chat;
                //if (team != null)
                //    chat = string.Format("<{0}{1}{2}> {3}", team.PlayerPrefix, client.Username, team.PlayerSuffix, packet.Message);
                //else
                    chat = new ChatMessage(string.Format("<{0}> {1}", client.Username, packet.Message));
                server.SendChat(chat);
            }
        }

        public static void KeepAlive(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            // TODO: Confirm value validity
            client.LastKeepAlive = DateTime.Now;
            client.Ping = (short)(client.LastKeepAlive - client.LastKeepAliveSent).TotalMilliseconds;
        }

        private static ServerStatus GetServerStatus(MinecraftServer server)
        {
            return new ServerStatus(
                new ServerStatus.ServerVersion(NetworkManager.FriendlyVersion, NetworkManager.ProtocolVersion),
                new ServerStatus.PlayerList(server.Settings.MaxPlayers, server.Clients.Count(c => c.IsLoggedIn),
                    server.Clients.Where(c => c.IsLoggedIn).Take(10).Select(p =>
                    new ServerStatus.PlayerList.Player(p.Username, p.Entity.EntityId.ToString())).ToArray()),
                server.Settings.MotD,
                ""); // TODO: Icon
        }
    }
}
