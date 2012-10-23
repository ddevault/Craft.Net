using System;
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
        }

        public override byte PacketId
        {
            get { return 0x05; }
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
                DataUtility.CreateInt16(SlotIndex),
                Item.GetData()));
        }
    }
}
