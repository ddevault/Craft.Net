using System;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
    public sealed class HeldItemChangePacket : Packet
    {
        private short SlotId;

        public override byte PacketId
        {
            get { return 0x10; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!DataUtility.TryReadInt16(buffer, ref offset, out SlotId))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (SlotId < 10 && SlotId >= 0)
            {
                client.Entity.SelectedSlot = (short)(PlayerEntity.InventoryHotbar + SlotId);
                var clients = server.EntityManager.GetKnownClients(client.Entity);
                foreach (var _client in clients)
                    _client.SendPacket(new EntityEquipmentPacket(client.Entity.Id, EntityEquipmentSlot.HeldItem,
                        client.Entity.Inventory [client.Entity.SelectedSlot]));
            }
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new NotImplementedException();
        }
    }
}