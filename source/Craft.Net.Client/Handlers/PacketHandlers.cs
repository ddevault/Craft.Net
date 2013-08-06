using System;
using Craft.Net.Networking;

namespace Craft.Net.Client.Handlers
{
    internal static class PacketHandlers
    {
        public static void Register(MinecraftClient client)
        {
            client.RegisterPacketHandler(EncryptionKeyRequestPacket.PacketId, LoginHandlers.EncryptionKeyRequest);
            client.RegisterPacketHandler(EncryptionKeyResponsePacket.PacketId, LoginHandlers.EncryptionKeyResponse);
            client.RegisterPacketHandler(LoginRequestPacket.PacketId, LoginHandlers.LoginRequest);
            client.RegisterPacketHandler(DisconnectPacket.PacketId, LoginHandlers.Disconnect);

            client.RegisterPacketHandler(PlayerPositionAndLookPacket.PacketId, EntityHandlers.PlayerPositionAndLook);

            client.RegisterPacketHandler(KeepAlivePacket.PacketId, KeepAlive);
            client.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatHandler.ChatMessage);

            client.RegisterPacketHandler(UpdateHealthPacket.PacketId, StateHandlers.UpdateHealth);
            client.RegisterPacketHandler(RespawnPacket.PacketId, StateHandlers.Respawn);

            client.RegisterPacketHandler(MapChunkBulkPacket.PacketId, WorldHandlers.MapChunkBulk);
            client.RegisterPacketHandler(ChunkDataPacket.PacketId, WorldHandlers.ChunkData);
        }

        public static void KeepAlive(MinecraftClient client, IPacket _packet)
        {
            client.SendPacket(_packet);
        }
    }
}

