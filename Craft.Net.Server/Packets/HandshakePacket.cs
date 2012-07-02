using System;
using System.Security.Cryptography;
using System.Net;

namespace Craft.Net.Server.Packets
{
    public class HandshakePacket : Packet
    {
        public byte ProtocolVersion;
        public string Username, Hostname;
        public int Port;

        public HandshakePacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x02;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadByte(Buffer, ref offset, out this.ProtocolVersion))
                return -1;
            if (!TryReadString(Buffer, ref offset, out this.Username))
                return -1;
            if (!TryReadString(Buffer, ref offset, out this.Hostname))
                return -1;
            if (!TryReadInt(Buffer, ref offset, out this.Port))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Console.WriteLine(Username + " logged into " + Hostname + ":" + Port +
                " with protocol version " + ProtocolVersion);
            if (ProtocolVersion < MinecraftServer.ProtocolVersion)
            {
                Client.SendPacket(new DisconnectPacket("Outdated client!"));
                Server.ProcessSendQueue();
                return;
            }
            if (ProtocolVersion > MinecraftServer.ProtocolVersion)
            {
                Client.SendPacket(new DisconnectPacket("Outdated server!"));
                Server.ProcessSendQueue();
                return;
            }
            Client.Username = Username;
            Client.Hostname = Hostname + ":" + Port.ToString();
            // Respond with encryption request
            Client.AuthenticationHash = CreateHash();
            EncryptionKeyRequestPacket keyRequest = 
                new EncryptionKeyRequestPacket(Client.AuthenticationHash,
                                               Server.KeyPair.getPublic());
            Client.SendPacket(keyRequest);
            Server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        private string CreateHash()
        {
            byte[] hash = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(new Random().Next()));
            string response = "";
            foreach (byte b in hash)
            {
                if (b < 0x10)
                    response += "0";
                response += b.ToString("x");
            }
            return response;
        }
    }
}

