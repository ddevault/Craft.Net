using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Client.Events;
using Craft.Net.Data;

namespace Craft.Net.Client.Handlers
{
    internal static class LoginHandlers
    {
        public static void EncryptionKeyRequest(MinecraftClient client, IPacket _packet)
        {
            var packet = (EncryptionKeyRequestPacket)_packet;
            var random = RandomNumberGenerator.Create();
            client.SharedSecret = new byte[16];
            random.GetBytes(client.SharedSecret); // Generate a secure AES key

            if (packet.ServerId != "-") // Online mode
            {
                // Authenticate with minecraft.net
                var data = Encoding.ASCII.GetBytes(packet.ServerId)
                    .Concat(client.SharedSecret)
                    .Concat(packet.PublicKey).ToArray();
                var hash = Cryptography.JavaHexDigest(data);
                var webClient = new WebClient();
                string result = webClient.DownloadString("http://session.minecraft.net/game/joinserver.jsp?user=" +
                    Uri.EscapeUriString(client.Session.Username) +
                    "&sessionId=" + Uri.EscapeUriString(client.Session.SessionId) +
                    "&serverId=" + Uri.EscapeUriString(hash));
                if (result != "OK")
                    LogProvider.Log("Unable to verify session: " + result);
            }

            var parser = new AsnKeyParser(packet.PublicKey);
            var key = parser.ParseRSAPublicKey();

            // Encrypt shared secret and verification token
            var crypto = new RSACryptoServiceProvider();
            crypto.ImportParameters(key);
            var encryptedSharedSecret = crypto.Encrypt(client.SharedSecret, false);
            var encryptedVerification = crypto.Encrypt(packet.VerificationToken, false);
            var response = new EncryptionKeyResponsePacket(encryptedSharedSecret, encryptedVerification);
            client.SendPacket(response);
        }

        public static void EncryptionKeyResponse(MinecraftClient client, IPacket _packet)
        {
            // Enable encryption
            client.Stream = new MinecraftStream(new AesStream(new BufferedStream(client.NetworkStream), client.SharedSecret));
            client.SendPacket(new ClientStatusPacket(ClientStatusPacket.ClientStatus.InitialSpawn));
            LogProvider.Log("Logged in.");
        }

        public static void LoginRequest(MinecraftClient client, IPacket _packet)
        {
            var packet = (LoginRequestPacket)_packet;
            client.EntityId = packet.EntityId;
            //client.Spawned = true;
            client.OnLoggedIn();

            // Initialize world
            client.World = new ReadOnlyWorld();
            client.LevelInformation = new LevelInformation(packet);
            client.OnWorldInitialized();
        }

        public static void Disconnect(MinecraftClient client, IPacket _packet)
        {
            var packet = (DisconnectPacket)_packet;
            LogProvider.Log("Disconnected: " + packet.Reason);
            client.OnDisconnected(new DisconnectEventArgs(packet.Reason));
        }
    }
}
