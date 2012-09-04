using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class TimeUpdatePacket : Packet
    {
        public long Time { get; set; }

        public TimeUpdatePacket()
        {
        }

        public TimeUpdatePacket(long time)
        {
            Time = time;
        }

        public override byte PacketId
        {
            get { return 0x4; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] payload = new byte[] { PacketId }
                .Concat(DataUtility.CreateInt64(0)) // TODO: What is this field for
                .Concat(DataUtility.CreateInt64(Time))
                .ToArray();
            client.SendData(payload);
        }
    }
}
