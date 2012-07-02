using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using java.security;
using Craft.Net.Server.Packets;

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

        internal BufferedBlockCipher Encrypter, Decrypter;
        internal Key SharedKey;
        internal int VerificationKey;
        internal int RecieveBufferIndex;
        internal byte[] RecieveBuffer;
        internal string AuthenticationHash;
        internal bool EncryptionEnabled;
        
        #endregion
        
        public MinecraftClient(Socket Socket)
        {
            this.Socket = Socket;
            this.RecieveBuffer = new byte[1024];
            this.RecieveBufferIndex = 0;
            this.SendQueue = new Queue<Packet>();
            this.IsDisconnected = false;
            this.IsLoggedIn = false;
            this.EncryptionEnabled = false;
        }

        public void SendPacket(Packet packet)
        {
            packet.PacketContext = PacketContext.ServerToClient;
            this.SendQueue.Enqueue(packet);
        }

        internal void SendData(byte[] Data)
        {
            if (this.EncryptionEnabled)
                Data = Encrypter.ProcessBytes(Data);
            this.Socket.BeginSend(Data, 0, Data.Length, SocketFlags.None, null, null);
        }
    }
}

