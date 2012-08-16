using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class EncryptionKeyRequestPacket : Packet
    {
        private readonly string authenticationHash;
        private readonly RSAParameters serverKey;

        public EncryptionKeyRequestPacket()
        {
        }

        public EncryptionKeyRequestPacket(string authenticationHash, RSAParameters serverKey)
        {
            this.authenticationHash = authenticationHash;
            this.serverKey = serverKey;
        }

        public override byte PacketId
        {
            get { return 0xFD; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            var verifyToken = new byte[4];
            var csp = new RNGCryptoServiceProvider();
            csp.GetBytes(verifyToken); // TODO: Encrypt this

            AsnKeyBuilder.AsnMessage encodedKey = AsnKeyBuilder.PublicKeyToX509(serverKey);

            byte[] buffer = new[] {PacketId}
                .Concat(DataUtility.CreateString(authenticationHash))
                .Concat(DataUtility.CreateInt16((short)encodedKey.GetBytes().Length))
                .Concat(encodedKey.GetBytes())
                .Concat(DataUtility.CreateInt16((short)verifyToken.Length))
                .Concat(verifyToken).ToArray();
            client.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
        }
    }
}