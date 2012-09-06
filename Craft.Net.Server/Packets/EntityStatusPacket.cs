using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum EntityStatus
    {
        Hurt = 2,
        Dead = 3,
        WolfTaming = 6,
        WolfTamed = 7,
        /// <summary>
        /// Shaking water off the wolf's body
        /// </summary>
        WolfShaking = 8,
        EatingAccepted = 9,
        /// <summary>
        /// Sheep eating grass
        /// </summary>
        SheepEating = 10
    }

    public class EntityStatusPacket : Packet
    {
        public int EntityId;
        public EntityStatus Status;

        public EntityStatusPacket()
        {
        }

        public EntityStatusPacket(int entityId, EntityStatus status)
        {
            EntityId = entityId;
            Status = status;
        }

        public override byte PacketId
        {
            get { return 0x26; }
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
                .Concat(new byte[] {(byte)Status}).ToArray();
            client.SendData(payload);
        }
    }
}
