using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Craft.Net.Networking;

namespace Craft.Net.Networking.Test
{
    [TestFixture]
    public class CryptoTest
    {
        [Test]
        public void TestAesStream()
        {
            var baseStream = new MemoryStream();
            var key = new byte[]
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            var encryptStream = new AesStream(baseStream, key);
            var plaintext = Encoding.UTF8.GetBytes("Hello, world!");
            encryptStream.Write(plaintext, 0, plaintext.Length);

            baseStream.Seek(0, SeekOrigin.Begin);
            var decryptStream = new AesStream(baseStream, key);
            byte[] buffer = new byte[plaintext.Length];
            decryptStream.Read(buffer, 0, buffer.Length);

            Assert.AreEqual("Hello, world!", Encoding.UTF8.GetString(buffer));
        }
    }
}
