using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Craft.Net.Metadata;
using NUnit.Framework;

namespace Craft.Net.Data.Test
{
    [TestFixture]
    public class MetadataTest
    {
        [Test]
        public void TestToString()
        {
            MetadataByte mByte = new MetadataByte(10, 200);
            var text = mByte.ToString();
            Assert.Pass("This test must be manually verified."); // TODO
        }

        [Test]
        public void TestReadDictionary()
        {
            byte[] data = new byte[] {0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x00, 0x7F};
            MetadataDictionary dictionary = MetadataDictionary.FromStream(new MinecraftStream(new MemoryStream(data)));
            var stream = new MemoryStream();
            dictionary.WriteTo(new MinecraftStream(stream));
            Assert.AreEqual(data, stream.GetBuffer());
        }
    }
}
