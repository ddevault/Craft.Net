using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
        private const string sessionCheckUri = "http://session.minecraft.net/game/checkserver.jsp?user={0}&serverId={1}";

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

        public static void ClientStatus(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var status = (ClientStatusPacket)packet;
            if (status.Status == ClientStatusPacket.ClientStatus.InitialSpawn)
            {
                // Create a hash for session verification
                SHA1 sha1 = SHA1.Create();
                AsnKeyBuilder.AsnMessage encodedKey = AsnKeyBuilder.PublicKeyToX509(server.ServerKey);
                byte[] shaData = Encoding.UTF8.GetBytes(client.AuthenticationHash)
                    .Concat(client.SharedKey)
                    .Concat(encodedKey.GetBytes()).ToArray();
                string hash = Cryptography.JavaHexDigest(shaData);

                // Talk to session.minecraft.net
                if (server.Settings.OnlineMode)
                {
                    var webClient = new WebClient();
                    var webReader = new StreamReader(webClient.OpenRead(
                        new Uri(string.Format(sessionCheckUri, client.Username, hash))));
                    string response = webReader.ReadToEnd();
                    webReader.Close();
                    if (response != "YES")
                    {
                        client.SendPacket(new DisconnectPacket("Failed to verify username!"));
                        return;
                    }
                }

                server.LogInPlayer(client);
            }
            else if (status.Status == ClientStatusPacket.ClientStatus.Respawn)
            {
                // TODO
            }
        }

        public static void ClientSettings(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var settings = (ClientSettingsPacket)packet;
            client.MaxViewDistance = (8 << settings.ViewDistance) + 2;
            // TODO: Colors enabled
            client.Entity.ShowCape = settings.ShowCape;
            try
            {
                client.Locale = CultureInfo.GetCultureInfo(settings.Locale.Replace("_", "-"));
            }
            catch
            {
                client.Locale = CultureInfo.InvariantCulture;
            }
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
