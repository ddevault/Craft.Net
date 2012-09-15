using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class UseBedPacket : Packet
    {
        private Vector3 HeadboardPosition;
        private int EntityId;

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
            client.SendData(CreateBuffer(
                DataUtility.CreateInt32(EntityId),
                new byte[] {0},
                DataUtility.CreateInt32((int)HeadboardPosition.X),
                new[] {(byte)HeadboardPosition.Y},
                DataUtility.CreateInt32((int)HeadboardPosition.Z)));
        }
    }
}
