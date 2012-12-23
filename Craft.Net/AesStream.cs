using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Craft.Net
{
    public class AesStream : Stream
    {
        private CryptoStream decryptStream { get; set; }
        private CryptoStream encryptStream { get; set; }
        internal byte[] Key { get; set; }

        public AesStream(Stream stream, byte[] key)
        {
            BaseStream = stream;
            Key = key;
            var rijndael = GenerateAES(key);
            var encryptTransform = rijndael.CreateEncryptor();
            var decryptTransform = rijndael.CreateDecryptor();

            encryptStream = new CryptoStream(BaseStream, encryptTransform, CryptoStreamMode.Write);
            decryptStream = new CryptoStream(BaseStream, decryptTransform, CryptoStreamMode.Read);
        }

        public Stream BaseStream { get; set; }

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
            return decryptStream.ReadByte();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return decryptStream.Read(buffer, offset, count);
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
            encryptStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            decryptStream.Close();
            encryptStream.Close();
        }

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
    }
}
