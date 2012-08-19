using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Server.Packets;
using Craft.Net.Data;
using Craft.Net.Data.Entities;
using Org.BouncyCastle.Crypto;

namespace Craft.Net.Server
{
    /// <summary>
    /// Describes a client connected to a <see cref="MinecraftServer"/>.
    /// </summary>
    public class MinecraftClient
    {
        #region Fields

        /// <summary>
        /// The client-provided chat modes.
        /// </summary>
        public ChatMode ChatMode;
        /// <summary>
        /// True if the client has enabled colors in chat.
        /// </summary>
        public bool ColorsEnabled;
        /// <summary>
        /// The entity this client represents.
        /// </summary>
        public PlayerEntity Entity;
        /// <summary>
        /// The speed at which the client is permitted to fly.
        /// </summary>
        public byte FlyingSpeed;
        /// <summary>
        /// The hostname the client connected with.
        /// </summary>
        public string Hostname;
        /// <summary>
        /// The client's current inventory.
        /// </summary>
        public Slot[] Inventory;
        public int SelectedSlot;
        /// <summary>
        /// Set to true if the client is currently crouching.
        /// </summary>
        public bool IsCrouching;
        /// <summary>
        /// Set to true if the client is currently sprinting.
        /// </summary>
        public bool IsSprinting;
        /// <summary>
        /// Set to true if the client has completed the login sequence and
        /// has been spawned.
        /// </summary>
        public bool IsLoggedIn;
        /// <summary>
        /// A list of all chunks the client has been sent and
        /// instructed to load.
        /// </summary>
        public List<Vector3> LoadedChunks;
        /// <summary>
        /// The client-provided locale string.
        /// </summary>
        public string Locale;
        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int MaxViewDistance;
        /// <summary>
        /// The time, in milliseconds, it takes this client to respond to a
        /// <see cref="KeepAlivePacket"/>.
        /// </summary>
        public short Ping;
        /// <summary>
        /// The current queue of packets to be sent to this client.
        /// </summary>
        public Queue<Packet> SendQueue;
        /// <summary>
        /// The <see cref="MinecraftServer"/> managing this client's connection.
        /// </summary>
        public MinecraftServer Server;
        /// <summary>
        /// The TCP socket used to communicate with this client.
        /// </summary>
        public Socket Socket; // TODO: Private?
        /// <summary>
        /// 3rd party client-specific data may be saved here.
        /// </summary>
        public Dictionary<string, object> Tags;
        /// <summary>
        /// This client's username.
        /// </summary>
        public string Username;
        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int ViewDistance;
        /// <summary>
        /// The speed at which this client is permitted to walk.
        /// </summary>
        public byte WalkingSpeed;

        internal List<int> KnownEntities;
        internal string AuthenticationHash;
        internal Timer KeepAliveTimer;
        internal DateTime LastKeepAlive, LastKeepAliveSent;
        internal BufferedBlockCipher Decrypter;
        internal BufferedBlockCipher Encrypter;
        internal bool EncryptionEnabled;
        internal byte[] SharedKey;
        internal bool ReadyToSpawn;
        internal byte[] RecieveBuffer;
        internal int RecieveBufferIndex;
        internal bool IsDisconnected;

        #endregion

        /// <summary>
        /// Creates a new MinecraftClient with the specified socket to be
        /// managed by the given <see cref="MinecraftServer"/>.
        /// </summary>
        public MinecraftClient(Socket socket, MinecraftServer server)
        {
            this.Socket = socket;
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
            this.Server = server;
            WalkingSpeed = 12;
            FlyingSpeed = 25;
            Inventory = new Slot[44];
            LastKeepAlive = DateTime.MaxValue.AddSeconds(-10);
            KnownEntities = new List<int>();
        }

        /// <summary>
        /// The maximum speed that a client may move.
        /// </summary>
        public double MaxMoveDistance
        {
            get
            {
                // TODO: Base this on speed
                return 1000;
            }
        }

        /// <summary>
        /// Queues the given packet for sending. Make sure to call
        /// <see cref="MinecraftServer.ProcessSendQueue"/> to send
        /// the queued packet.
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(Packet packet)
        {
            packet.PacketContext = PacketContext.ServerToClient;
            SendQueue.Enqueue(packet);
        }

        /// <summary>
        /// Sends the specified raw data to the client. This data
        /// will be encrypted if encryption is enabled.
        /// </summary>
        public void SendData(byte[] data)
        {
            if (IsDisconnected)
                return;
#if DEBUG
            Server.Log(DataUtility.DumpArray(data), LogImportance.Low);
#endif
            if (EncryptionEnabled)
                data = Encrypter.ProcessBytes(data);
            Socket.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
        }

        /// <summary>
        /// Asyncronously updates chunks loaded on the client
        /// </summary>
        /// <returns></returns>
        public Task UpdateChunksAsync()
        {
            if ((int)(Entity.Position.X) >> 4 != (int)(Entity.OldPosition.X) >> 4 ||
                (int)(Entity.Position.Z) >> 4 != (int)(Entity.OldPosition.Z) >> 4)
            {
                return Task.Factory.StartNew(() => UpdateChunks(true));
            }
            return null;
        }

        /// <summary>
        /// Asyncronously updates chunks loaded on the client and forces a
        /// recalculation of which chunks should be loaded.
        /// </summary>
        public Task ForceUpdateChunksAsync()
        {
            return Task.Factory.StartNew(() => UpdateChunks(true));
        }

        /// <summary>
        /// Updates which chunks are loaded on the client.
        /// </summary>
        public void UpdateChunks(bool forceUpdate)
        {
            if (forceUpdate ||
                (int)(Entity.Position.X) >> 4 != (int)(Entity.OldPosition.X) >> 4 ||
                (int)(Entity.Position.Z) >> 4 != (int)(Entity.OldPosition.Z) >> 4
                )
            {
                var newChunks = new List<Vector3>();
                for (int x = -ViewDistance; x < ViewDistance; x++)
                    for (int z = -ViewDistance; z < ViewDistance; z++)
                    {
                        newChunks.Add(new Vector3(
                                          ((int)Entity.Position.X >> 4) + x,
                                          0,
                                          ((int)Entity.Position.Z >> 4) + z));
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

        /// <summary>
        /// Loads the given chunk on the client.
        /// </summary>
        public void LoadChunk(Vector3 position)
        {
            World world = Server.GetClientWorld(this);
            Chunk chunk = world.GetChunk(position);
            var dataPacket = new ChunkDataPacket(ref chunk);
            SendPacket(dataPacket);
            this.LoadedChunks.Add(position);
        }

        /// <summary>
        /// Unloads the given chunk on the client.
        /// </summary>
        public void UnloadChunk(Vector3 position)
        {
            var dataPacket = new ChunkDataPacket();
            dataPacket.AddBitMap = 0;
            dataPacket.GroundUpContiguous = true;
            dataPacket.PrimaryBitMap = 0;
            dataPacket.X = (int)position.X;
            dataPacket.Z = (int)position.Z;
            dataPacket.CompressedData = ChunkDataPacket.ChunkRemovalSequence;
            SendPacket(dataPacket);
            this.LoadedChunks.Remove(position);
        }

        /// <summary>
        /// Sends a <see cref="ChatMessagePacket"/> to the client.
        /// </summary>
        public void SendChat(string message)
        {
            SendPacket(new ChatMessagePacket(message));
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
            if (LastKeepAlive.AddSeconds(30) < DateTime.Now && false)
            {
                //Server.Log("Client timed out");
                //IsDisconnected = true;
                // TODO: Fix this
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