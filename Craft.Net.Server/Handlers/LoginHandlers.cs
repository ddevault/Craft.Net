using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Craft.Net.Data;

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
            client.Username = handshake.Username;
            client.Hostname = handshake.ServerHostname + ":" + handshake.ServerPort;
            if (server.Settings.OnlineMode)
                client.AuthenticationHash = CreateHash();
            else
                client.AuthenticationHash = "-";
            if (server.Settings.EnableEncryption)
                client.SendPacket(CreateEncryptionRequest(client, server));
            else
                server.LogInPlayer(client);
        }

        public static void EncryptionKeyResponse(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var response = (EncryptionKeyResponsePacket)packet;
            client.SharedKey = server.CryptoServiceProvider.Decrypt(response.SharedSecret, false);
            client.SendPacket(new EncryptionKeyResponsePacket(new byte[0], new byte[0]));
        }

        private static EncryptionKeyRequestPacket CreateEncryptionRequest(MinecraftClient client, MinecraftServer server)
        {
            var verifyToken = new byte[4];
            var csp = new RNGCryptoServiceProvider();
            csp.GetBytes(verifyToken);
            verifyToken = server.CryptoServiceProvider.Encrypt(verifyToken, false);
            // TODO: Confirm verify token validity

            var encodedKey = AsnKeyBuilder.PublicKeyToX509(server.ServerKey);
            var request = new EncryptionKeyRequestPacket(client.AuthenticationHash,
                encodedKey.GetBytes(), verifyToken);
            return request;
        }

        private static string CreateHash()
        {
            byte[] hash = BitConverter.GetBytes(MathHelper.Random.Next());
            string response = "";
            foreach (byte b in hash)
            {
                if (b < 0x10)
                    response += "0";
                response += b.ToString("x");
            }
            return response;
        }
    }
}
