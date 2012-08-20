using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class SetSlotPacket : Packet
    {
        public byte WindowId;
        public short Index;
        public Slot Slot;

        public SetSlotPacket()
        {
        }

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

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] payload = new byte[] { PacketId, WindowId }
                .Concat(DataUtility.CreateInt16(Index))
                .Concat(Slot.GetData()).ToArray();
            client.SendData(payload);
        }
    }
}
