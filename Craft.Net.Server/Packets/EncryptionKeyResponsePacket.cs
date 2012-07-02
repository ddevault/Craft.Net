using System;
using javax.crypto;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using javax.crypto.spec;

namespace Craft.Net.Server
{
    public class EncryptionKeyResponsePacket : Packet
    {
        public byte[] SharedSecret, VerifyToken;

        public EncryptionKeyResponsePacket()
        {
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
            Console.WriteLine("Preparing encryption.");
            Cipher cipher = Cipher.getInstance("RSA");
            cipher.init(Cipher.DECRYPT_MODE, Server.KeyPair.getPrivate());
            Client.SharedKey = new SecretKeySpec(cipher.doFinal(SharedSecret), "AES-128");
            Console.WriteLine("Decrypted shared key, preparing encrypter.");
            Client.Encrypter = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
            Client.Encrypter.Init(true,
                   new ParametersWithIV(new KeyParameter(Client.SharedKey.getEncoded()), 
                   Client.SharedKey.getEncoded(), 0, 16));
            Console.WriteLine("Encrypter ready, preparing decrypter.");
            Client.Decrypter = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
            Client.Decrypter.Init(false,
                   new ParametersWithIV(new KeyParameter(Client.SharedKey.getEncoded()), 
                   Client.SharedKey.getEncoded(), 0, 16));

            Client.SendPacket(new EncryptionKeyResponsePacket());
            Console.WriteLine("Encryption ready.");
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            // Send packet and enable encryption
        }
    }
}
