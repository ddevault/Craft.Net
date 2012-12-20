using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data.Entities;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class SpawnObjectPacket : Packet
    {
        public ObjectEntity Entity;

        public SpawnObjectPacket()
        {
        }

        public SpawnObjectPacket(ObjectEntity entity)
        {
            Entity = entity;
        }

        public override byte PacketId
        {
            get { return 0x17; }
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
            if (Entity.Data > 0)
            {
                client.SendData(CreateBuffer(
                    DataUtility.CreateInt32(Entity.Id),
                    new[] { Entity.EntityType },
                    DataUtility.CreateAbsoluteInteger(Entity.Position.X),
                    DataUtility.CreateAbsoluteInteger(Entity.Position.Y),
                    DataUtility.CreateAbsoluteInteger(Entity.Position.Z),
                    DataUtility.CreatePackedByte(Entity.Yaw),
                    DataUtility.CreatePackedByte(Entity.Pitch),
                    DataUtility.CreateInt32(Entity.Data),
                    DataUtility.CreateInt16((short)Entity.Velocity.X),
                    DataUtility.CreateInt16((short)Entity.Velocity.Y),
                    DataUtility.CreateInt16((short)Entity.Velocity.Z)));
            }
            else
            {
                client.SendData(CreateBuffer(
                    DataUtility.CreateInt32(Entity.Id),
                    new[] { Entity.EntityType },
                    DataUtility.CreateAbsoluteInteger(Entity.Position.X),
                    DataUtility.CreateAbsoluteInteger(Entity.Position.Y),
                    DataUtility.CreateAbsoluteInteger(Entity.Position.Z),
                    DataUtility.CreatePackedByte(Entity.Yaw),
                    DataUtility.CreatePackedByte(Entity.Pitch),
                    DataUtility.CreateInt32(Entity.Data)));
            }
        }
    }
}
