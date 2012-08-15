using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Worlds;
using Craft.Net.Server.Worlds.Entities;

namespace Craft.Net.Server
{
    public class MinecraftClient
    {
        private const int BufferSize = 1024;

        #region Fields

        internal string AuthenticationHash;
        public ChatMode ChatMode;
        public bool ColorsEnabled;
        internal ICryptoTransform Decrypter;
        internal ICryptoTransform Encrypter;
        internal bool EncryptionEnabled;
        public PlayerEntity Entity;
        public byte FlyingSpeed;
        public string Hostname;
        public Slot[] Inventory;
        public bool IsCrouching;
        public bool IsDisconnected;
        public bool IsLoggedIn;
        public bool IsSprinting;
        internal Timer KeepAliveTimer;
        internal DateTime LastKeepAlive, LastKeepAliveSent;
        public List<Vector3> LoadedChunks;
        public string Locale;

        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int MaxViewDistance;

        public short Ping;
        internal bool ReadyToSpawn;
        internal byte[] RecieveBuffer;
        internal int RecieveBufferIndex;
        public Queue<Packet> SendQueue;
        public MinecraftServer Server;
        internal byte[] SharedKey;
        public Socket Socket;
        public Dictionary<string, object> Tags;
        public string Username;
        internal int VerificationKey;

        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int ViewDistance;

        public byte WalkingSpeed;

        #endregion

        public MinecraftClient(Socket Socket, MinecraftServer Server)
        {
            this.Socket = Socket;
            RecieveBuffer = new byte[1024];
            RecieveBufferIndex = 0;
            SendQueue = new Queue<Packet>();
            IsDisconnected = false;
            IsLoggedIn = false;
            EncryptionEnabled = false;
            Locale = "en_US";
            MaxViewDistance = 10;
            ViewDistance = 3;
            ReadyToSpawn = false;
            LoadedChunks = new List<Vector3>();
            this.Server = Server;
            WalkingSpeed = 12;
            FlyingSpeed = 25;
            Inventory = new Slot[44];
            LastKeepAlive = DateTime.MaxValue.AddSeconds(-120);
        }

        public double MaxMoveDistance
        {
            get
            {
                // TODO: Base this on speed
                return 1000;
            }
        }

        public void SendPacket(Packet packet)
        {
            packet.PacketContext = PacketContext.ServerToClient;
            SendQueue.Enqueue(packet);
        }

        public void SendData(byte[] Data)
        {
#if DEBUG
            Server.Log(DumpArray(Data), LogImportance.Low);
#endif
            if (EncryptionEnabled)
            {
                var output = new byte[Data.Length];
                Encrypter.TransformBlock(Data, 0, output.Length, output, 0);
                Socket.BeginSend(output, 0, output.Length, SocketFlags.None, null, null);
            }
            else
            {
                Socket.BeginSend(Data, 0, Data.Length, SocketFlags.None, null, null);
            }
        }

        public static string DumpArray(byte[] array)
        {
            // TODO: There's probably somewhere better to put this
            // Maybe a general utility class?
            if (array.Length == 0)
                return "[]";
            var sb = new StringBuilder((array.Length*2) + 2);
            foreach (byte b in array)
            {
                sb.AppendFormat("0x{0},", b.ToString("x"));
            }
            return sb.ToString().Remove(sb.Length - 1) + "]";
        }

        public Task UpdateChunksAsync()
        {
            if ((int) (Entity.Position.X) >> 4 != (int) (Entity.OldPosition.X) >> 4 ||
                (int) (Entity.Position.Z) >> 4 != (int) (Entity.OldPosition.Z) >> 4)
            {
                return Task.Factory.StartNew(() => UpdateChunks(true));
            }
            return null;
        }

        public Task ForceUpdateChunksAsync()
        {
            return Task.Factory.StartNew(() => UpdateChunks(true));
        }

        public void UpdateChunks(bool ForceUpdate)
        {
            if (ForceUpdate ||
                (int) (Entity.Position.X) >> 4 != (int) (Entity.OldPosition.X) >> 4 ||
                (int) (Entity.Position.Z) >> 4 != (int) (Entity.OldPosition.Z) >> 4
                )
            {
                var newChunks = new List<Vector3>();
                for (int x = -ViewDistance; x < ViewDistance; x++)
                    for (int z = -ViewDistance; z < ViewDistance; z++)
                    {
                        newChunks.Add(new Vector3(
                                          ((int) Entity.Position.X >> 4) + x,
                                          0,
                                          ((int) Entity.Position.Z >> 4) + z));
                    }
                // Unload extraneous columns
                var currentChunks = new List<Vector3>(LoadedChunks);
                foreach (Vector3 chunk in currentChunks)
                {
                    if (!newChunks.Contains(chunk))
                        UnloadChunk(chunk);
                }
                // Load new columns
                foreach (Vector3 chunk in newChunks)
                {
                    if (!LoadedChunks.Contains(chunk))
                        LoadChunk(chunk);
                }
            }
        }

        public void LoadChunk(Vector3 position)
        {
            World world = Server.GetClientWorld(this);
            Chunk chunk = world.GetChunk(position);
            var dataPacket = new ChunkDataPacket(ref chunk);
            SendPacket(dataPacket);
        }

        public void UnloadChunk(Vector3 position)
        {
            var dataPacket = new ChunkDataPacket();
            dataPacket.AddBitMap = 0;
            dataPacket.GroundUpContiguous = true;
            dataPacket.PrimaryBitMap = 0;
            dataPacket.X = (int) position.X;
            dataPacket.Z = (int) position.Z;
            dataPacket.CompressedData = ChunkDataPacket.ChunkRemovalSequence;
            SendPacket(dataPacket);
        }

        public void SendChat(string Message)
        {
            SendPacket(new ChatMessagePacket(Message));
            Server.ProcessSendQueue();
        }

        internal void StartKeepAliveTimer()
        {
            KeepAliveTimer = new Timer(KeepAlive, null, 30000, 30000);
        }

        internal void KeepAlive(object unused)
        {
            // Keep alive timer is also responsible for trickling in chunks
            // early in the session.
            if (ReadyToSpawn && ViewDistance < MaxViewDistance)
            {
                ViewDistance++;
                ForceUpdateChunksAsync(); // TODO: Move this to its own timer
            }
            if (LastKeepAlive.AddSeconds(60) < DateTime.Now)
            {
                Server.Log("Client timed out");
                IsDisconnected = true;
            }
            else
            {
                SendPacket(new KeepAlivePacket(MinecraftServer.Random.Next()));
                Server.ProcessSendQueue();
                LastKeepAliveSent = DateTime.Now;
            }
        }
    }
}