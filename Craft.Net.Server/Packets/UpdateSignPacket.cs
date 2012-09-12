using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class UpdateSignPacket : Packet
    {
        public Vector3 Position;
        public string Text1;
        public string Text2;
        public string Text3;
        public string Text4;

        public override byte PacketId
        {
            get { return 0x82; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            int x, z;
            short y;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out x))
                return -1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out y))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out z))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text1))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text2))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text3))
                return -1;
            if (!DataUtility.TryReadString(buffer, ref offset, out Text4))
                return -1;
            Position = new Vector3(x, y, z);
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            //
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] payload = new byte[] { PacketId }
                .Concat(DataUtility.CreateInt32((int)Position.X))
                .Concat(DataUtility.CreateInt16((short)Position.Y))
                .Concat(DataUtility.CreateInt32((int)Position.Z))
                .Concat(DataUtility.CreateString(Text1))
                .Concat(DataUtility.CreateString(Text2))
                .Concat(DataUtility.CreateString(Text3))
                .Concat(DataUtility.CreateString(Text4)).ToArray();
            client.SendData(payload);
        }
    }
}
