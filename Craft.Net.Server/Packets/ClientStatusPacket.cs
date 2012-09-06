using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public enum ClientStatus
    {
        InitialSpawn = 0,
        Respawn = 1
    }

    public class ClientStatusPacket : Packet
    {
        private const string sessionCheckUri = "http://session.minecraft.net/game/checkserver.jsp?user={0}&serverId={1}";

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

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
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
                    break;
                case ClientStatus.Respawn:
                    var world = server.GetClientWorld(client);
                    client.Entity.Position = new Vector3(
                        client.Entity.SpawnPoint.X,
                        client.Entity.SpawnPoint.Y + PlayerEntity.Height,
                        client.Entity.SpawnPoint.Z);
                    client.Entity.Health = client.Entity.MaxHealth;
                    client.Entity.Food = 20;
                    client.Entity.FoodSaturation = 20;
                    server.EntityManager.SpawnEntity(server.GetClientWorld(client), client.Entity);
                    //client.SendPacket(new UpdateHealthPacket(client.Entity.Health, client.Entity.Food, client.Entity.FoodSaturation));
                    client.SendPacket(new RespawnPacket(Dimension.Overworld, server.Difficulty,
                        client.Entity.GameMode, world.LevelType));
                    client.SendPacket(new PlayerPositionAndLookPacket(
                                  client.Entity.Position, client.Entity.Yaw, client.Entity.Pitch, true));
                    server.ProcessSendQueue();
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