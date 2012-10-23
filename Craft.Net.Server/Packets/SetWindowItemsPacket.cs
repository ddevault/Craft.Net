using System;
using System.Linq;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum WindowType
    {
        Inventory = 0,
        Chest = 0,
        Workbench = 1,
        Furnace = 2,
        Dispenser = 3,
        EnchantmentTable = 4
    }

    public class SetWindowItemsPacket : Packet
    {
        public byte WindowId { get; set; }
        public Slot[] Slots { get; set; }

        public SetWindowItemsPacket()
        {
        }

        public SetWindowItemsPacket(byte windowId, Slot[] slots)
        {
            WindowId = windowId;
            Slots = slots;
        }

        public override byte PacketId
        {
            get { return 0x68; }
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
            byte[] payload = CreateBuffer(
                new[] {WindowId},
                DataUtility.CreateInt16((short)Slots.Length));
            foreach (var slot in Slots)
                payload = payload.Concat(slot.GetData()).ToArray();

            client.SendData(payload);
        }
    }
}
