using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Craft.Net.Server.Test
{
    [TestFixture]
    public class PacketTests
    {
        [Test]
        public void TestPacketList()
        {
            // Test order
            for (int i = 1; i < PacketReader.PacketTypes.Length; i++)
            {
                if (PacketReader.PacketTypes[i] == null || PacketReader.PacketTypes[i - 1] == null)
                    continue;
                Assert.Less(((Packet)Activator.CreateInstance(PacketReader.PacketTypes[i - 1])).PacketId,
                            ((Packet)Activator.CreateInstance(PacketReader.PacketTypes[i])).PacketId);
            }
            // Test completeness
            var packetTypes = Assembly.GetAssembly(typeof(Packet)).GetTypes().Where(t =>
                !t.IsAbstract && typeof(Packet).IsAssignableFrom(t)).OrderBy(t => ((Packet)Activator.CreateInstance(t)).PacketId);
            string values = "";
            // Tests for completeness and generates passing code in case of failure
            for (int i = 0; i <= 0xFF; i++)
            {
                var type = packetTypes.FirstOrDefault(p => ((Packet)Activator.CreateInstance(p)).PacketId == i);
                if (type == null)
                    values += "null,";
                else
                    values += "typeof(" + type.Name + "),";
                values += " // 0x" + i.ToString("x") + "\n";
            }
            for (int i = 0; i <= 0xFF; i++)
            {
                var type = packetTypes.FirstOrDefault(p => ((Packet)Activator.CreateInstance(p)).PacketId == i);
                if (type == null)
                    Assert.IsNull(PacketReader.PacketTypes[i]);
                else
                    Assert.AreEqual(type, PacketReader.PacketTypes[i]);
            }
        }
    }
}
