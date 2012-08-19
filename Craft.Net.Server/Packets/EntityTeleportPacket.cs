using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class EntityTeleportPacket : Packet
    {
        public int EntityId;
        public Vector3 Position;
        public float Yaw, Pitch;

        public EntityTeleportPacket()
        {
        }

        public EntityTeleportPacket(Entity entity)
        {
            EntityId = entity.Id;
            Position = entity.Position;
            this.Yaw = entity.Yaw;
            this.Pitch = entity.Pitch;
        }

        public override byte PacketId
        {
            get { return 0x22; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            throw new InvalidOperationException();
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] payload = new byte[] { PacketId }
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.X))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.Y))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.Z))
                .Concat(DataUtility.CreatePackedByte(Yaw))
                .Concat(DataUtility.CreatePackedByte(Pitch)).ToArray();
            client.SendData(payload);
        }
    }
}
