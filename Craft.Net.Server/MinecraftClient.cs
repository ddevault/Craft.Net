using System;
using System.Net.Sockets;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using java.security;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Worlds.Entities;
using Craft.Net.Server.Worlds;
using System.Threading;

namespace Craft.Net.Server
{
    public class MinecraftClient
    {
        private const int BufferSize = 1024;
        
        #region Fields
        
        public Socket Socket;
        public string Username, Hostname;
        public Queue<Packet> SendQueue;
        public bool IsDisconnected;
        public bool IsLoggedIn;
        public PlayerEntity Entity;
        public string Locale;
        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int ViewDistance, MaxViewDistance;
        public short Ping;
        public ChatMode ChatMode;
        public bool ColorsEnabled;
        public List<Vector3> LoadedChunks;
        public Dictionary<string, object> Tags;
        public MinecraftServer Server;
        public byte WalkingSpeed, FlyingSpeed;
        public bool IsCrouching, IsSprinting;
        public Slot[] Inventory;

        internal BufferedBlockCipher Encrypter, Decrypter;
        internal Key SharedKey;
        internal int VerificationKey;
        internal int RecieveBufferIndex;
        internal byte[] RecieveBuffer;
        internal string AuthenticationHash;
        internal bool EncryptionEnabled, ReadyToSpawn;
        internal Timer KeepAliveTimer;
        internal DateTime LastKeepAlive, LastKeepAliveSent;
        internal int ExpectedKeepAlive;

        #endregion

        public double MaxMoveDistance
        {
            get
            {
                // TODO: Base this on speed
                return 4;
            }
        }
        
        public MinecraftClient(Socket Socket, MinecraftServer Server)
        {
            this.Socket = Socket;
            this.RecieveBuffer = new byte[1024];
            this.RecieveBufferIndex = 0;
            this.SendQueue = new Queue<Packet>();
            this.IsDisconnected = false;
            this.IsLoggedIn = false;
            this.EncryptionEnabled = false;
            this.Locale = "en_US";
            this.MaxViewDistance = 10;
            this.ViewDistance = 3;
            this.ReadyToSpawn = false;
            this.LoadedChunks = new List<Vector3>();
            this.Server = Server;
            this.WalkingSpeed = 12;
            this.FlyingSpeed = 25;
            this.Inventory = new Slot[44];
        }

        public void SendPacket(Packet packet)
        {
            packet.PacketContext = PacketContext.ServerToClient;
            this.SendQueue.Enqueue(packet);
        }

        public void SendData(byte[] Data)
        {
#if DEBUG
            Server.Log(DumpArray(Data), LogImportance.Low);
#endif
            if (this.EncryptionEnabled)
                Data = Encrypter.ProcessBytes(Data);
            this.Socket.BeginSend(Data, 0, Data.Length, SocketFlags.None, null, null);
        }

        public static string DumpArray(byte[] array)
        {
            // TODO: There's probably somewhere better to put this
            // Maybe a general utility class?
            if (array.Length == 0)
                return "[]";
            string dump = "[";
            foreach (byte b in array)
            {
                dump += "0x" + b.ToString("x") + ",";
            }
            return dump.Remove(dump.Length - 1) + "]";
        }

        public void UpdateChunksAsync()
        {
            if ((int)(this.Entity.Position.X) >> 4 != (int)(this.Entity.OldPosition.X) >> 4 ||
                (int)(this.Entity.Position.Z) >> 4 != (int)(this.Entity.OldPosition.Z) >> 4)
            {
                Thread t = new Thread(UpdateChunks);
                t.Start();
            }
        }

        public void UpdateChunks()
        {
            UpdateChunks(false);
        }

        public void UpdateChunks(bool ForceUpdate)
        {
            if ((int)(this.Entity.Position.X) >> 4 != (int)(this.Entity.OldPosition.X) >> 4 ||
                (int)(this.Entity.Position.Z) >> 4 != (int)(this.Entity.OldPosition.Z) >> 4 ||
                ForceUpdate)
            {
                List<Vector3> newChunks = new List<Vector3>();
                for (int x = -this.ViewDistance; x < this.ViewDistance; x++)
                    for (int z = -this.ViewDistance; z < this.ViewDistance; z++)
                {
                    newChunks.Add(new Vector3(
                        ((int)this.Entity.Position.X >> 4) + x,
                        0,
                        ((int)this.Entity.Position.Z >> 4) + z));
                }
                // Unload extraneous columns
                List<Vector3> currentChunks = new List<Vector3>(this.LoadedChunks);
                foreach (Vector3 chunk in currentChunks)
                {
                    if (!newChunks.Contains(chunk))
                        UnloadChunk(chunk);
                }
                // Load new columns
                foreach (Vector3 chunk in newChunks)
                {
                    if (!this.LoadedChunks.Contains(chunk))
                        LoadChunk(chunk);
                }
                Server.ProcessSendQueue();
            }
        }

        public void LoadChunk(Vector3 position)
        {
            World world = Server.GetClientWorld(this);
            Chunk chunk = world.GetChunk(position);
            ChunkDataPacket dataPacket = new ChunkDataPacket(ref chunk);
            this.SendPacket(dataPacket);
        }

        public void UnloadChunk(Vector3 position)
        {
            ChunkDataPacket dataPacket = new ChunkDataPacket();
            dataPacket.AddBitMap = 0;
            dataPacket.GroundUpContiguous = true;
            dataPacket.PrimaryBitMap = 0;
            dataPacket.X = (int)position.X;
            dataPacket.Z = (int)position.Z;
            dataPacket.CompressedData = ChunkDataPacket.ChunkRemovalSequence;
            this.SendPacket(dataPacket);
        }

        internal void KeepAlive(object unused)
        {
            if (LastKeepAlive.AddSeconds(30) < DateTime.Now)
                this.IsDisconnected = true;
            else
            {
                this.SendPacket(new KeepAlivePacket(MinecraftServer.Random.Next()));
                this.Server.ProcessSendQueue();
                this.LastKeepAliveSent = DateTime.Now;
            }
        }
    }
}

