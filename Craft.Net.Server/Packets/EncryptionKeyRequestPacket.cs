using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Craft.Net.Server.Packets
{
    public class EncryptionKeyRequestPacket : Packet
    {
        public string AuthenticationHash;
        public RSAParameters ServerKey;

        public EncryptionKeyRequestPacket()
        {
        }

        public EncryptionKeyRequestPacket(string AuthenticationHash, RSAParameters ServerKey)
        {
            this.AuthenticationHash = AuthenticationHash;
            this.ServerKey = ServerKey;
        }

        public override byte PacketID
        {
            get
            {
                return 0xFD;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] verifyToken = new byte[4];
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            csp.GetBytes(verifyToken); // TODO: Encrypt this

            AsnKeyBuilder.AsnMessage encodedKey = AsnKeyBuilder.PublicKeyToX509(ServerKey);

            byte[] buffer = new byte[] { PacketID }
                .Concat(CreateString(AuthenticationHash))
                .Concat(CreateShort((short)encodedKey.GetBytes().Length))
                .Concat(encodedKey.GetBytes())
                .Concat(CreateShort((short)verifyToken.Length))
                .Concat(verifyToken).ToArray();
            Client.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
        }
    }
}

