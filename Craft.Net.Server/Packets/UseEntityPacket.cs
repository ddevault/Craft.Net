using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class UseEntityPacket : Packet
    {
        public int UserId;
        public int TargetId;
        public bool LeftClick;

        public override byte PacketId
        {
            get { return 0x7; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out UserId))
                return -1;
            if (!DataUtility.TryReadInt32(buffer, ref offset, out TargetId))
                return -1;
            if (!DataUtility.TryReadBoolean(buffer, ref offset, out LeftClick))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            var item = client.Entity.SelectedItem;
            
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}
