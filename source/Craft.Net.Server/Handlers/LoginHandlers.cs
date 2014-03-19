using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Logic;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Craft.Net.Server.Handlers
{
    internal static class LoginHandlers
    {
        private const string sessionCheckUri = "https://sessionserver.mojang.com/session/minecraft/hasJoined?username={0}&serverId={1}";

        public static void Handshake(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (HandshakePacket)_packet;
            if (packet.ProtocolVersion < NetworkManager.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated client!"));
                return;
            }
            if (packet.ProtocolVersion > NetworkManager.ProtocolVersion)
            {
                client.SendPacket(new DisconnectPacket("Outdated server!"));
                return;
            }
            client.Hostname = packet.ServerHostname + ":" + packet.ServerPort;
        }

        public static void LoginStart(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (LoginStartPacket)_packet;
            if (server.Clients.Any(c => c.IsLoggedIn && c.Username == packet.Username))
                client.Disconnect("You're already on this server!");
            else
            {
                client.Username = packet.Username;
                if (server.Settings.OnlineMode)
                    client.ServerId = CreateId();
                else
                {
                    client.ServerId = CreateId();
                    client.UUID = Guid.NewGuid().ToJavaUUID();
                }
                if (server.Settings.EnableEncryption)
                   client.SendPacket(CreateEncryptionRequest(client, server));
                else
                    server.LogInPlayer(client);
            }
        }

        public static void EncryptionKeyResponse(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (EncryptionKeyResponsePacket)_packet;
            var decryptedToken = server.CryptoServiceProvider.Decrypt(packet.VerificationToken, false);
            for (int i = 0; i < decryptedToken.Length; i++)
            {
                if (decryptedToken[i] != client.VerificationToken[i])
                {
                    client.Disconnect("Unable to authenticate.");
                    return;
                }
            }
            client.SharedKey = server.CryptoServiceProvider.Decrypt(packet.SharedSecret, false);
            // Create a hash for session verification
            AsnKeyBuilder.AsnMessage encodedKey = AsnKeyBuilder.PublicKeyToX509(server.ServerKey);
            byte[] shaData = Encoding.UTF8.GetBytes(client.ServerId)
                .Concat(client.SharedKey)
                .Concat(encodedKey.GetBytes()).ToArray();
            string hash = Cryptography.JavaHexDigest(shaData);

            // Talk to sessionserver.minecraft.net
            if (server.Settings.OnlineMode)
            {
                var webClient = new WebClient();
                var webReader = new StreamReader(webClient.OpenRead(
                    new Uri(string.Format(sessionCheckUri, client.Username, hash))));
                string response = webReader.ReadToEnd();
                webReader.Close();
                var json = JToken.Parse(response);
                if (string.IsNullOrEmpty(response))
                {
                    client.Disconnect("Failed to verify username!");
                    return;
                }
                client.UUID = json["id"].Value<string>();
            }
            client.NetworkStream = new AesStream(client.NetworkClient.GetStream(), client.SharedKey);
            client.NetworkManager.BaseStream = client.NetworkStream;
            client.EncryptionEnabled = true;
            var eventArgs = new ConnectionEstablishedEventArgs(client);
            server.OnConnectionEstablished(eventArgs);
            if (eventArgs.PermitConnection)
                server.LogInPlayer(client);
            else
                client.Disconnect(eventArgs.DisconnectReason);
        }

        public static void ClientStatus(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ClientStatusPacket)_packet;
            if (packet.Change == ClientStatusPacket.StatusChange.Respawn)
            {
                var world = client.Entity.World;
                client.Entity.Position = new Vector3(
                    client.Entity.SpawnPoint.X,
                    // FIXME: This seems to drop the player camera from half the height of a login spawn
                    client.Entity.SpawnPoint.Y,
                    client.Entity.SpawnPoint.Z);
                client.Entity.Health = client.Entity.MaxHealth;
                client.Entity.Food = 20;
                client.Entity.FoodSaturation = 20;
                server.EntityManager.SpawnEntity(world, client.Entity);
                client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));
                client.SendPacket(new RespawnPacket(Dimension.Overworld, server.Settings.Difficulty, client.GameMode, world.WorldGenerator.GeneratorName));
                client.SendPacket(new PlayerPositionAndLookPacket(client.Entity.Position.X, client.Entity.Position.Y, client.Entity.Position.Z,
                    client.Entity.Position.Y + PlayerEntity.Height, client.Entity.Yaw, client.Entity.Pitch, true));
            }
        }

        public static void ClientSettings(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (ClientSettingsPacket)_packet;
            client.Settings.MaxViewDistance = (32 << packet.ViewDistance) + 2;
            // TODO: Colors enabled
            client.Settings.ShowCape = packet.ShowCape;
            try
            {
                client.Settings.Locale = CultureInfo.GetCultureInfo(packet.Locale.Replace("_", "-"));
            }
            catch
            {
                client.Settings.Locale = CultureInfo.InvariantCulture;
            }
        }

        private static EncryptionKeyRequestPacket CreateEncryptionRequest(RemoteClient client, MinecraftServer server)
        {
            var verifyToken = new byte[4];
            var csp = new RNGCryptoServiceProvider();
            csp.GetBytes(verifyToken);
            client.VerificationToken = verifyToken;

            var encodedKey = AsnKeyBuilder.PublicKeyToX509(server.ServerKey);
            var request = new EncryptionKeyRequestPacket(client.ServerId,
                encodedKey.GetBytes(), verifyToken);
            return request;
        }

        private static string CreateId()
        {
            var random = RandomNumberGenerator.Create();
            byte[] data = new byte[8];
            random.GetBytes(data);
            var response = "";
            foreach (byte b in data)
                response += b.ToString("X2");
            return response;
        }
    }
}
