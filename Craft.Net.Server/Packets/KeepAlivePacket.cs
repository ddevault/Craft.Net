using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class KeepAlivePacket : Packet
    {
        public int KeepAlive;

        public KeepAlivePacket()
        {
        }

        public KeepAlivePacket(int keepAlive)
        {
            this.KeepAlive = keepAlive;
        }

        public override byte PacketId
        {
            get { return 0x00; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out KeepAlive))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            client.LastKeepAlive = DateTime.Now;
            client.Ping = (short)(client.LastKeepAlive - client.LastKeepAliveSent).TotalMilliseconds;
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(DataUtility.CreateInt32(KeepAlive)));
        }
    }
}