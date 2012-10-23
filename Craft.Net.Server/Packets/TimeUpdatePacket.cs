using System;
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
            get { return 0x04; }
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
            client.SendData(CreateBuffer(
                DataUtility.CreateInt64(0), //TODO update this to the latest packet definition
                DataUtility.CreateInt64(Time)));
        }
    }
}
