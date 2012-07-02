using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Math;
using System.Net;
using System.IO;

namespace Craft.Net.Server.Packets
{
    public enum ClientStatus
    {
        InitialSpawn = 0,
        Respawn = 2
    }

    public class ClientStatusPacket : Packet
    {
        const string SessionCheckUri = "http://session.minecraft.net/game/checkserver.jsp?user={0}&serverId={1}";

        public ClientStatus ClientStatus;

        public ClientStatusPacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xCD;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            byte status = 0;
            if (!TryReadByte(Buffer, ref offset, out status))
                return -1;
            ClientStatus = (ClientStatus)status;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Console.WriteLine("Client status: " + ClientStatus.ToString());
            switch (ClientStatus)
            {
                case ClientStatus.InitialSpawn:
                    SHA1 sha1 = SHA1.Create();
                    byte[] shaData = Encoding.ASCII.GetBytes(Client.AuthenticationHash)
                        .Concat(Server.KeyPair.getPublic().getEncoded())
                        .Concat(Client.SharedKey.getEncoded()).ToArray();
                    byte[] hash = sha1.ComputeHash(shaData);

                    WebClient webClient = new WebClient();
                    StreamReader webReader = new StreamReader(webClient.OpenRead(
                            new Uri(string.Format(SessionCheckUri,
                            Client.Username, GetHashString(hash)))));
                    string response = webReader.ReadToEnd();
                    webReader.Close();

                    Console.WriteLine(string.Format(SessionCheckUri,
                            Client.Username, GetHashString(hash)) + "\n" + response);

                    if (response != "YES")
                        Client.SendPacket(new DisconnectPacket("Failed to verify username!"));
                    else
                        Client.SendPacket(new LoginPacket());
                    Server.ProcessSendQueue();
                    break;
                case ClientStatus.Respawn:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        private string GetHashString(byte[] data)
        {
            BigInteger bigInt = new BigInteger(data);
            return bigInt.ToString(16);
        }
    }
}

