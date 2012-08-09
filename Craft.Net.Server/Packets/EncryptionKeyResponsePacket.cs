using System;
using System.Linq;
using System.Security.Cryptography;

namespace Craft.Net.Server.Packets
{
    public class EncryptionKeyResponsePacket : Packet
    {
        public byte[] SharedSecret, VerifyToken;

        public EncryptionKeyResponsePacket()
        {
            SharedSecret = new byte[0];
            VerifyToken = new byte[0];
        }

        public override byte PacketID
        {
            get
            {
                return 0xFC;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            short secretLength = 0, verifyLength = 0;
            int offset = 1;
            if (!TryReadShort(Buffer, ref offset, out secretLength))
                return -1;
            if (!TryReadArray(Buffer, secretLength, ref offset, out this.SharedSecret))
                return -1;
            if (!TryReadShort(Buffer, ref offset, out verifyLength))
                return -1;
            if (!TryReadArray(Buffer, verifyLength, ref offset, out this.VerifyToken))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Client.SharedKey = Server.CryptoServiceProvider.Decrypt(SharedSecret, false);

            Client.Encrypter = Cryptography.GenerateAES(Client.SharedKey).CreateEncryptor();

            Client.Decrypter = Cryptography.GenerateAES(Client.SharedKey).CreateDecryptor();

            Client.SendPacket(new EncryptionKeyResponsePacket());
            Server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            // Send packet and enable encryption
            byte[] buffer = new byte[] { PacketID }.Concat(
                CreateShort((short)SharedSecret.Length)).Concat(
                SharedSecret).Concat(
                CreateShort((short)VerifyToken.Length)).Concat(
                VerifyToken).ToArray();
            Client.SendData(buffer);
            Client.EncryptionEnabled = true;
        }
    }
}
