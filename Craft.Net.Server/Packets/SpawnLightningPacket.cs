using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class SpawnLightningPacket : Packet
    {
        public int EntityId;
        public Vector3 Position;

        public SpawnLightningPacket()
        {
        }

        public SpawnLightningPacket(int entityId, Vector3 position)
        {
            EntityId = entityId;
            Position = position;
        }

        public override byte PacketId
        {
            get { return 0x47; }
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
            byte[] payload = new byte[] { PacketId }
                .Concat(DataUtility.CreateInt32(EntityId))
                .Concat(DataUtility.CreateBoolean(true))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.X))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.Y))
                .Concat(DataUtility.CreateAbsoluteInteger(Position.Z)).ToArray();
            client.SendData(payload);
        }
    }
}
