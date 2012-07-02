using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class KeepAlivePacket : Packet
    {
        public int KeepAlive;

        public KeepAlivePacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0x00;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadInt(Buffer, ref offset, out KeepAlive))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Client.LastKeepAlive = DateTime.Now;
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }.Concat(
                CreateInt(KeepAlive)).ToArray();
            Client.SendData(buffer);
        }
    }
}

