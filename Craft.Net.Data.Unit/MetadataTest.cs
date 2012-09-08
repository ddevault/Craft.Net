using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Metadata;
using NUnit.Framework;

namespace Craft.Net.Data.Unit
{
    [TestFixture]
    public class MetadataTest
    {
        [Test]
        public void TestToString()
        {
            MetadataByte mByte = new MetadataByte(10, 200);
            var text = mByte.ToString();
            Assert.Pass("This test must be manually verified.");
        }

        [Test]
        public void TestReadDictionary()
        {
            byte[] data = new byte[] {0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x00, 0x7F};
            MetadataDictionary dictionary;
            int offset = 0;
            MetadataDictionary.TryReadMetadata(data, ref offset, out dictionary);
            Assert.AreEqual(data, dictionary.Encode());
            offset = 0;
            Assert.IsFalse(MetadataDictionary.TryReadMetadata(data.Take(5).ToArray(), ref offset, out dictionary));
        }
    }
}
