using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class UseBedPacket : Packet
    {
        public Vector3 HeadboardPosition;
        public int EntityId;

        public UseBedPacket()
        {
        }

        public UseBedPacket(int entityId, Vector3 headboardPosition)
        {
            EntityId = entityId;
            HeadboardPosition = headboardPosition;
        }

        public override byte PacketId
        {
            get { return 0x11; }
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
            byte[] payload = new byte[] {PacketId}
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(new byte[] {0})
                .Concat(DataUtility.CreateInt32((int)HeadboardPosition.X))
                .Concat(new byte[] { (byte)HeadboardPosition.Y })
                .Concat(DataUtility.CreateInt32((int)HeadboardPosition.Z))
                .ToArray();
            client.SendData(payload);
        }
    }
}
