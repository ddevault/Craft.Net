using System;
using Craft.Net.Networking;

namespace Craft.Net.Client.Handlers
{
    internal static class PacketHandlers
    {
        public static void Register(MinecraftClient client)
        {
            client.RegisterPacketHandler(typeof(EncryptionKeyRequestPacket), LoginHandlers.EncryptionKeyRequest);
            client.RegisterPacketHandler(typeof(LoginSuccessPacket), LoginHandlers.LoginSuccess);
            client.RegisterPacketHandler(typeof(JoinGamePacket), LoginHandlers.JoinGame);
            client.RegisterPacketHandler(typeof(DisconnectPacket), LoginHandlers.Disconnect);

            client.RegisterPacketHandler(typeof(PlayerPositionAndLookPacket), EntityHandlers.PlayerPositionAndLook);

            client.RegisterPacketHandler(typeof(KeepAlivePacket), KeepAlive);
            client.RegisterPacketHandler(typeof(ChatMessagePacket), ChatHandler.ChatMessage);

            client.RegisterPacketHandler(typeof(UpdateHealthPacket), StateHandlers.UpdateHealth);
            client.RegisterPacketHandler(typeof(RespawnPacket), StateHandlers.Respawn);

            client.RegisterPacketHandler(typeof(MapChunkBulkPacket), WorldHandlers.MapChunkBulk);
            client.RegisterPacketHandler(typeof(ChunkDataPacket), WorldHandlers.ChunkData);
            client.RegisterPacketHandler(typeof(BlockChangePacket), WorldHandlers.BlockChange);
        }

        public static void KeepAlive(MinecraftClient client, IPacket _packet)
        {
            client.SendPacket(_packet);
        }
    }
}

