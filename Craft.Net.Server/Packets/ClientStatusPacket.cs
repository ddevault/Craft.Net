using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Math;
using System.Net;
using System.IO;
using Craft.Net.Server.Worlds.Entities;
using System.Threading;
using Craft.Net.Server.Worlds;

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
            switch (ClientStatus)
            {
                case ClientStatus.InitialSpawn:
                    // Create a hash for session verification
                    SHA1 sha1 = SHA1.Create();
                    byte[] shaData = Encoding.UTF8.GetBytes(Client.AuthenticationHash)
                        .Concat(Client.SharedKey.getEncoded())
                        .Concat(Server.KeyPair.getPublic().getEncoded()).ToArray();
                    byte[] hash = sha1.ComputeHash(shaData);

                    // Talk to session.minecraft.net
                    if (Server.OnlineMode)
                    {
                        WebClient webClient = new WebClient();
                        StreamReader webReader = new StreamReader(webClient.OpenRead(
                                new Uri(string.Format(SessionCheckUri,
                                Client.Username, GetHashString(hash)))));
                        string response = webReader.ReadToEnd();
                        webReader.Close();
                        if (response != "YES")
                        {
                            Client.SendPacket(new DisconnectPacket("Failed to verify username!"));
                            return;
                        }
                    }

                    Server.Log(Client.Username + " logged in.");
                    // Spawn player
                    Client.Entity = new PlayerEntity(Client);
                    Client.Entity.Position = Server.DefaultWorld.SpawnPoint;
                    Client.Entity.Position += new Vector3(0, PlayerEntity.Height, 0);
                    Server.DefaultWorld.EntityManager.SpawnEntity(Client.Entity);
                    Client.SendPacket(new LoginPacket(Client.Entity.Id,
                           Server.DefaultWorld.LevelType, Server.DefaultWorld.GameMode,
                           Client.Entity.Dimension, Server.DefaultWorld.Difficulty,
                           Server.MaxPlayers));

                    // Send initial chunks
                    Client.UpdateChunks(true);
                    Client.SendPacket(new PlayerPositionAndLookPacket(
                        Client.Entity.Position, Client.Entity.Yaw, Client.Entity.Pitch, true));
                    Client.KeepAliveTimer = new Timer(Client.KeepAlive, Client, 30000, 30000);
                    Client.ReadyToSpawn = true;

                    Server.UpdatePlayerList(null); // Should also process send queue
                    break;
                case ClientStatus.Respawn:
                    // TODO
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

