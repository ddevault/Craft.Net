using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
#if MONO
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
#endif

namespace Craft.Net.Networking
{
    public class AesStream : Stream
    {
#if MONO
        private BufferedBlockCipher encryptCipher { get; set; }
        private BufferedBlockCipher decryptCipher { get; set; }
#else
        private CryptoStream decryptStream { get; set; }
        private CryptoStream encryptStream { get; set; }
#endif
        internal byte[] Key { get; set; }

        public AesStream(Stream stream, byte[] key)
        {
            BaseStream = stream;
            Key = key;
#if MONO
            encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            encryptCipher.Init(true, new ParametersWithIV(
                new KeyParameter(key), key, 0, 16));
            decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            decryptCipher.Init(false, new ParametersWithIV(
                new KeyParameter(key), key, 0, 16));
#else
            var rijndael = GenerateAES(key);
            var encryptTransform = rijndael.CreateEncryptor();
            var decryptTransform = rijndael.CreateDecryptor();
            encryptStream = new CryptoStream(BaseStream, encryptTransform, CryptoStreamMode.Write);
            decryptStream = new CryptoStream(BaseStream, decryptTransform, CryptoStreamMode.Read);
#endif
        }

        public Stream BaseStream { get; set; }

#if !MONO
        private static Rijndael GenerateAES(byte[] key)
        {
            var cipher = new RijndaelManaged();
            cipher.Mode = CipherMode.CFB;
            cipher.Padding = PaddingMode.None;
            cipher.KeySize = 128;
            cipher.FeedbackSize = 8;
            cipher.Key = cipher.IV = key;
            return cipher;
        }
#endif

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { return 0; } // hack for libnbt
            set { throw new NotSupportedException(); }
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int ReadByte()
        {
#if MONO
            int value = BaseStream.ReadByte();
            if (value == -1) return value;
            return decryptCipher.ProcessByte((byte)value)[0];
#else
            return decryptStream.ReadByte();
#endif
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
#if MONO
            int length = BaseStream.Read(buffer, offset, count);
            var decrypted = decryptCipher.ProcessBytes(buffer, offset, count);
            Array.Copy(decrypted, offset, buffer, offset, count);
            return length;
#else
            return decryptStream.Read(buffer, offset, count);
#endif
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
#if MONO
            var encrypted = encryptCipher.ProcessBytes(buffer, offset, count);
            BaseStream.Write(encrypted, 0, encrypted.Length);
#else
            encryptStream.Write(buffer, offset, count);
#endif
        }

        public override void Close()
        {
#if MONO
            BaseStream.Close();
#else
            decryptStream.Close();
            encryptStream.Close();
            BaseStream.Close();
#endif
        }

#if !MONO // TODO
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return decryptStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return encryptStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return decryptStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            encryptStream.EndWrite(asyncResult);
        }
#endif
    }
}
