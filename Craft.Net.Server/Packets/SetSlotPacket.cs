using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class SetSlotPacket : Packet
    {
        private byte WindowId;
        private short Index;
        private Slot Slot;

        public SetSlotPacket(byte windowId, short index, Slot slot)
        {
            WindowId = windowId;
            Index = index;
            Slot = slot;
        }

        public override byte PacketId
        {
            get { return 0x67; }
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
                new[] {WindowId},
                DataUtility.CreateInt16(Index),
                Slot.GetData()));
        }
    }
}
