using System;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class EntityRelativeMovePacket : Packet
    {
        public int EntityId;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public EntityRelativeMovePacket()
        {
        }

        public EntityRelativeMovePacket(Entity entity)
        {
            EntityId = entity.Id;
            Vector3 diff = entity.Position - entity.OldPosition;
            this.DeltaX = (sbyte)(diff.X * 32.0);
            this.DeltaY = (sbyte)(diff.Y * 32.0);
            this.DeltaZ = (sbyte)(diff.Z * 32.0);
        }

        public override byte PacketId
        {
            get { return 0x1F; }
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
                new[] {
                    (byte)DeltaX,
                    (byte)DeltaY,
                    (byte)DeltaZ
                }));
        }
    }
}
