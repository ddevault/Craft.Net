using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Handlers
{
    /// <summary>
    /// Packet handlers for login, handshake, encryption, etc
    /// </summary>
    internal static class LoginHandlers
    {
        private const string sessionCheckUri = "http://session.minecraft.net/game/checkserver.jsp?user={0}&serverId={1}";

        public static void Handshake(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (HandshakePacket)_packet;
            if (packet.ProtocolVersion < PacketReader.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated client!"));
                return;
            }
            if (packet.ProtocolVersion > PacketReader.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated server!"));
                return;
            }
            if (server.Clients.Any(c => c.Username == packet.Username))
            {
                client.SendPacket(new DisconnectPacket(""));
                return;
            }
            client.Username = packet.Username;
            client.Hostname = packet.ServerHostname + ":" + packet.ServerPort;
            if (server.Settings.OnlineMode)
                client.AuthenticationHash = CreateHash();
            else
                client.AuthenticationHash = "-";
            if (server.Settings.EnableEncryption)
                client.SendPacket(CreateEncryptionRequest(client, server));
            else
                server.LogInPlayer(client);
        }

        public static void EncryptionKeyResponse(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (EncryptionKeyResponsePacket)_packet;
            client.SharedKey = server.CryptoServiceProvider.Decrypt(packet.SharedSecret, false);
            client.SendPacket(new EncryptionKeyResponsePacket(new byte[0], new byte[0]));
        }

        public static void ClientStatus(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ClientStatusPacket)_packet;
            if (packet.Status == ClientStatusPacket.ClientStatus.InitialSpawn)
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
            else if (packet.Status == ClientStatusPacket.ClientStatus.Respawn)
            {
                var world = client.World;
                client.Entity.Position = new Vector3(
                    client.Entity.SpawnPoint.X,
                    client.Entity.SpawnPoint.Y + PlayerEntity.Height,
                    client.Entity.SpawnPoint.Z);
                client.Entity.Health = client.Entity.MaxHealth;
                client.Entity.Food = 20;
                client.Entity.FoodSaturation = 20;
                server.EntityManager.SpawnEntity(world, client.Entity);
                client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));
                client.SendPacket(new RespawnPacket(Dimension.Overworld, server.Settings.Difficulty, client.Entity.GameMode, World.Height, world.LevelType));
                client.SendPacket(new PlayerPositionAndLookPacket(client.Entity.Position.X, client.Entity.Position.Y, client.Entity.Position.Z,
                    client.Entity.Position.Y + PlayerEntity.Height, client.Entity.Yaw, client.Entity.Pitch, true));
            }
        }

        public static void ClientSettings(MinecraftClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ClientSettingsPacket)_packet;
            client.MaxViewDistance = (8 << packet.ViewDistance) + 2;
            // TODO: Colors enabled
            client.Entity.ShowCape = packet.ShowCape;
            try
            {
                client.Locale = CultureInfo.GetCultureInfo(packet.Locale.Replace("_", "-"));
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
            // verifyToken = server.CryptoServiceProvider.Encrypt(verifyToken, false);
            // TODO: I think I'm encrypting that wrong
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
                response += b.ToString("x2");
            return response;
        }
    }
}
