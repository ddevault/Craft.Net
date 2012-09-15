using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class EntityVelocityPacket : Packet
    {
        private int EntityId { get; set; }
        private Vector3 Velocity { get; set; }

        /// <summary>
        /// Velocity is measured in blocks per second.
        /// </summary>
        public EntityVelocityPacket(int entityId, Vector3 velocity)
        {
            EntityId = entityId;
            Velocity = velocity;
        }

        public override byte PacketId
        {
            get { return 0x1C; }
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
            var x = (short)(Velocity.X * 1600);
            var y = (short)(Velocity.Y * 1600);
            var z = (short)(Velocity.Z * 1600);

            client.SendData(CreateBuffer(
                DataUtility.CreateInt32(EntityId),
                DataUtility.CreateInt16(x),
                DataUtility.CreateInt16(y),
                DataUtility.CreateInt16(z)));
        }
    }
}
