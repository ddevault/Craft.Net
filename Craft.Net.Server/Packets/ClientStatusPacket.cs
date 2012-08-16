using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum ClientStatus
    {
        InitialSpawn = 0,
        Respawn = 2
    }

    public class ClientStatusPacket : Packet
    {
        private const string SessionCheckUri = "http://session.minecraft.net/game/checkserver.jsp?user={0}&serverId={1}";

        public ClientStatus ClientStatus;

        public override byte PacketId
        {
            get { return 0xCD; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte status = 0;
            if (!DataUtility.TryReadByte(buffer, ref offset, out status))
                return -1;
            ClientStatus = (ClientStatus)status;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            switch (ClientStatus)
            {
                case ClientStatus.InitialSpawn:
                    // Create a hash for session verification
                    SHA1 sha1 = SHA1.Create();
                    AsnKeyBuilder.AsnMessage encodedKey = AsnKeyBuilder.PublicKeyToX509(server.ServerKey);
                    byte[] shaData = Encoding.UTF8.GetBytes(client.AuthenticationHash)
                        .Concat(client.SharedKey)
                        .Concat(encodedKey.GetBytes()).ToArray();
                    string hash = Cryptography.JavaHexDigest(shaData);

                    // Talk to session.minecraft.net
                    if (server.OnlineMode)
                    {
                        var webClient = new WebClient();
                        var webReader = new StreamReader(webClient.OpenRead(
                            new Uri(string.Format(SessionCheckUri,
                                                  client.Username, hash))));
                        string response = webReader.ReadToEnd();
                        webReader.Close();
                        if (response != "YES")
                        {
                            client.SendPacket(new DisconnectPacket("Failed to verify username!"));
                            return;
                        }
                    }

                    server.LogInPlayer(client);
                    break;
                case ClientStatus.Respawn:
                    // TODO
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}