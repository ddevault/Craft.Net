using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            MinecraftClient.RegisterPacketHandler(KeepAlivePacket.PacketId, KeepAlive);
            MinecraftClient.RegisterPacketHandler(ChatMessagePacket.PacketId, ChatMessage);
        }

        public static void KeepAlive(MinecraftClient client, IPacket _packet)
        {
            client.SendPacket(_packet);
        }

        public static void ChatMessage(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            Console.WriteLine(packet.Message);
        }
    }
}
