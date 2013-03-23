using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Client.Events;

namespace Craft.Net.Client.Handlers
{
    internal static class PacketHandlers
    {
        /// <summary>
        /// Registers all built-in packet handlers.
        /// </summary>
        public static void RegisterHandlers()
        {
            MinecraftClient.RegisterPacketHandler(EncryptionKeyRequestPacket.PacketId, LoginHandlers.EncryptionKeyRequest);
            MinecraftClient.RegisterPacketHandler(EncryptionKeyResponsePacket.PacketId, LoginHandlers.EncryptionKeyResponse);
            MinecraftClient.RegisterPacketHandler(LoginRequestPacket.PacketId, LoginHandlers.LoginRequest);
            MinecraftClient.RegisterPacketHandler(DisconnectPacket.PacketId, LoginHandlers.Disconnect);

            MinecraftClient.RegisterPacketHandler(PlayerPositionAndLookPacket.PacketId, EntityHandlers.PlayerPositionAndLook);

            MinecraftClient.RegisterPacketHandler(KeepAlivePacket.PacketId, KeepAlive);
            MinecraftClient.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatMessage);

            MinecraftClient.RegisterPacketHandler(UpdateHealthPacket.PacketId, StateHandlers.UpdateHealth);
            MinecraftClient.RegisterPacketHandler(RespawnPacket.PacketId, StateHandlers.Respawn);

            MinecraftClient.RegisterPacketHandler(MapChunkBulkPacket.PacketId, WorldHandlers.MapChunkBulk);
            MinecraftClient.RegisterPacketHandler(ChunkDataPacket.PacketId, WorldHandlers.ChunkData);
        }

        public static void KeepAlive(MinecraftClient client, IPacket _packet)
        {
            client.SendPacket(_packet);
        }

        public static void ChatMessage(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            LogProvider.Log(packet.Message, LogImportance.High);
            client.OnChatMessage(new ChatMessageEventArgs(packet.Message));
        }
    }
}
