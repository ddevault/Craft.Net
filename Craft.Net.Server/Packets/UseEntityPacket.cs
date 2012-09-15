using System;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public class UseEntityPacket : Packet
    {
        public int UserId;
        public int TargetId;
        public bool LeftClick;

        public override byte PacketId
        {
            get { return 0x07; }
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
            var target = server.EntityManager.GetEntity(TargetId);
            if (target == null || 
                server.EntityManager.GetEntityWorld(target) != server.EntityManager.GetEntityWorld(client.Entity) ||
                target.Position.DistanceTo(client.Entity.Position) > 6) // TODO: client.Reach
                return;

            if (target is LivingEntity)
            {
                // Do damage
                var livingEntity = target as LivingEntity;

                var item = client.Entity.SelectedItem.Item;
                if (item == null)
                    item = new AirBlock();
                livingEntity.Damage(item.AttackDamage);
                livingEntity.Velocity /*+*/ = DataUtility.RotateY(new Vector3(0, 0, client.IsSprinting ? 10 : 3), // TODO: Knockback enchantment
                    DataUtility.DegreesToRadians(client.Entity.Yaw));                  // TODO: Physics
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}
