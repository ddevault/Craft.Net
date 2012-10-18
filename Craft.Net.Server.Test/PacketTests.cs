using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Craft.Net.Data;
using Craft.Net.Server.Packets;
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
            Console.WriteLine("Expected array code:\n" + values);
            for (int i = 0; i <= 0xFF; i++)
            {
                var type = packetTypes.FirstOrDefault(p => ((Packet)Activator.CreateInstance(p)).PacketId == i);
                if (type == null)
                    Assert.IsNull(PacketReader.PacketTypes[i]);
                else
                    Assert.AreEqual(type, PacketReader.PacketTypes[i]);
            }
        }

        [Test]
        public void TestSoundEffects()
        {
            var server = new MinecraftServer(new IPEndPoint(IPAddress.Loopback, 25565));
            server.AddLevel(new Level());
            server.Settings.MotD = "Sound effect test";
            server.Settings.OnlineMode = false;
            server.Start();
            bool success = true;
            string failedSound = "n/a";
            DateTime inconclusiveTime = DateTime.Now.AddSeconds(100);

            Queue<string> effects = new Queue<string>();
            Thread test = null;

            foreach (var effect in typeof(SoundEffect).GetFields().Where(f => f.FieldType == typeof(string) && f.IsLiteral))
            {
                effects.Enqueue(effect.GetValue(new SoundEffect()) as string);
            }

            server.PlayerLoggedIn += (s ,e) =>
                {
                    e.Client.SendChat("Beginning sound effect test in 5 seconds. Type \"fail\" into chat to indicate failure.");
                    inconclusiveTime = DateTime.MaxValue;
                    test = new Thread(new ThreadStart(() =>
                        {
                            Thread.Sleep(5000);
                            while (effects.Any())
                            {
                                e.Client.SendChat("Playing sound: " + effects.Peek());
                                e.Client.SendPacket(new NamedSoundEffectPacket(effects.Peek(), e.Client.Entity.Position));
                                Thread.Sleep(5000);
                                effects.Dequeue();
                            }
                        }));
                    test.Start();
                    e.Handled = true;
                };

            server.PlayerLoggedOut += (s, e) =>
                {
                    test.Abort();
                    server.Stop();
                    success = false;
                    failedSound = "Player left before test completion.";
                    effects = new Queue<string>();
                    e.Handled = true;
                    Assert.Fail("Player left before test completion.");
                };

            server.ChatMessage += (s, e) =>
                {
                    if (e.RawMessage == "fail")
                    {
                        test.Abort();
                        server.Stop();
                        failedSound = effects.Peek();
                        effects = new Queue<string>();
                        success = false;
                        Assert.Fail("Sound effect: " + effects.Peek());
                    }
                };

            while (effects.Count != 0 && DateTime.Now < inconclusiveTime) { Thread.Sleep(100); }
            if (DateTime.Now >= inconclusiveTime)
                Assert.Inconclusive("No player joined within 10 second time limit.");
            else
            {
                if (success)
                    Assert.Pass();
                else
                    Assert.Fail("Failed sound effect: " + failedSound);
            }
        }
    }
}
