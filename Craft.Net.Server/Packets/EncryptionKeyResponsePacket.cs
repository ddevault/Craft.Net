using System.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Craft.Net.Data;

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

        public override byte PacketId
        {
            get { return 0xFC; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            short secretLength = 0, verifyLength = 0;
            int offset = 1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out secretLength))
                return -1;
            if (!DataUtility.TryReadArray(buffer, secretLength, ref offset, out SharedSecret))
                return -1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out verifyLength))
                return -1;
            if (!DataUtility.TryReadArray(buffer, verifyLength, ref offset, out VerifyToken))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            client.SharedKey = server.CryptoServiceProvider.Decrypt(SharedSecret, false);

            client.Encrypter = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            client.Encrypter.Init(true,
                                  new ParametersWithIV(new KeyParameter(client.SharedKey), client.SharedKey, 0, 16));

            client.Decrypter = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            client.Decrypter.Init(false,
                                  new ParametersWithIV(new KeyParameter(client.SharedKey), client.SharedKey, 0, 16));

            client.SendPacket(new EncryptionKeyResponsePacket());
            server.ProcessSendQueue();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            // Send packet and enable encryption
            byte[] buffer = new[] {PacketId}.Concat(
                DataUtility.CreateInt16((short)SharedSecret.Length)).Concat(
                SharedSecret).Concat(
                DataUtility.CreateInt16((short)VerifyToken.Length)).Concat(
                VerifyToken).ToArray();
            client.SendData(buffer);
            client.EncryptionEnabled = true;
        }
    }
}