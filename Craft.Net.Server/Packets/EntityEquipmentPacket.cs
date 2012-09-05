using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum EntityEquipmentSlot
    {
        HeldItem = 0,
        Headgear = 1,
        Chestplate = 2,
        Pants = 3,
        Footwear = 4
    }

    public class EntityEquipmentPacket : Packet
    {
        public int EntityId { get; set; }
        /// <summary>
        /// Note: Zero is used for the currently held item
        /// </summary>
        public short SlotIndex { get; set; }
        public Slot Item { get; set; }

        public EntityEquipmentPacket()
        {
        }

        public EntityEquipmentPacket(int entityId, EntityEquipmentSlot slot, Slot item)
        {
            EntityId = entityId;
            SlotIndex = (short)slot;
            Item = item;
            if (slot == 0 && item.Id != 0xFFFF)
                item.Id = 0xFFFF;
        }

        public override byte PacketId
        {
            get { return 0x5; }
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
                .Concat(DataUtility.CreateInt16(SlotIndex))
                .Concat(Item.GetData()).ToArray();
            client.SendData(payload);
        }
    }
}
