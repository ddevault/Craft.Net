using System;
using System.Net.Sockets;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using java.security;
using Craft.Net.Server.Packets;
using Craft.Net.Server.Worlds.Entities;
using Craft.Net.Server.Worlds;

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
        public DateTime LastKeepAlive;
        public PlayerEntity Entity;
        public string Locale;
        /// <summary>
        /// The view distance in chunks.
        /// </summary>
        public int ViewDistance, MaxViewDistance;
        public ChatMode ChatMode;
        public bool ColorsEnabled;
        public List<Vector3> LoadedChunks;
        public Dictionary<string, object> Tags;
        public MinecraftServer Server;

        internal BufferedBlockCipher Encrypter, Decrypter;
        internal Key SharedKey;
        internal int VerificationKey;
        internal int RecieveBufferIndex;
        internal byte[] RecieveBuffer;
        internal string AuthenticationHash;
        internal bool EncryptionEnabled, ReadyToSpawn;
        
        #endregion
        
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
            this.ViewDistance = 8;
            this.ReadyToSpawn = false;
            this.Server = Server;
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
            if (array.Length == 0)
                return "[]";
            string dump = "[";
            foreach (byte b in array)
            {
                dump += "0x" + b.ToString("x") + ",";
            }
            return dump.Remove(dump.Length - 1) + "]";
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

                if (ViewDistance < MaxViewDistance)
                    ViewDistance++;
            }
        }
    }
}

