using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Client.Events;
using Craft.Net.Networking;
using Craft.Net.Common;
using System.Security.Authentication;

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
                if (!client.Session.OnlineMode)
                    throw new AuthenticationException("Server is in online mode, but client is in offline mode.");
                var data = Encoding.ASCII.GetBytes(packet.ServerId)
                    .Concat(client.SharedSecret)
                    .Concat(packet.PublicKey).ToArray();
                var hash = Cryptography.JavaHexDigest(data);
                var webClient = new WebClient();
                string result = webClient.DownloadString("http://session.minecraft.net/game/joinserver.jsp?user=" +
                    Uri.EscapeUriString(client.Session.SelectedProfile.Name) +
                    "&sessionId=" + Uri.EscapeUriString(client.Session.SessionId) +
                    "&serverId=" + Uri.EscapeUriString(hash));
                if (result != "OK")
                {
                    // TODO
                }
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
            client.FlushPackets();
            client.NetworkManager.BaseStream = new AesStream(client.NetworkStream, client.SharedSecret);
        }

        public static void LoginSuccess(MinecraftClient client, IPacket _packet)
        {
            // var packet = (LoginSuccessPacket)_packet;
            // TODO: We might want to do something here, dunno
        }

        public static void JoinGame(MinecraftClient client, IPacket _packet)
        {
            var packet = (JoinGamePacket)_packet;
            // TODO: We might want to store the other packet fields somewhere
            client.EntityId = packet.EntityId;
            client.IsLoggedIn = true;
            client.World = new ReadOnlyWorld();
            client.OnLoggedIn();
        }

        public static void Disconnect(MinecraftClient client, IPacket _packet)
        {
            var packet = (DisconnectPacket)_packet;
            client.OnDisconnected(new DisconnectEventArgs(packet.Reason));
        }
    }
}