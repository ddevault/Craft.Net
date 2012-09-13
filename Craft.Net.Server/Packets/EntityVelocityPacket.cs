using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class EntityVelocityPacket : Packet
    {
        public int EntityId { get; set; }
        public Vector3 Velocity { get; set; }

        public EntityVelocityPacket()
        {
        }

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
            byte[] payload = new byte[] {PacketId}
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(DataUtility.CreateInt16(x))
                .Concat(DataUtility.CreateInt16(y))
                .Concat(DataUtility.CreateInt16(z)).ToArray();
            client.SendData(payload);
        }
    }
}
