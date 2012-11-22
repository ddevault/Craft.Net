using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class CollectItemPacket : Packet
    {
        public int CollectedItemId;
        public int CollectorId;

        public CollectItemPacket()
        {
        }

        public CollectItemPacket(int collectedItemId, int collectorId)
        {
            CollectedItemId = collectedItemId;
            CollectorId = collectorId;
        }

        public override byte PacketId
        {
            get { return 0x16; }
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
                DataUtility.CreateInt32(CollectedItemId),
                DataUtility.CreateInt32(CollectorId)));
            }
        }
}