using System;
using System.Linq;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class DestroyEntityPacket : Packet
    {
        public int[] EntityIds;

        public DestroyEntityPacket()
        {
        }

        public DestroyEntityPacket(params int[] entities) // TODO: I don't really like this
        {
            EntityIds = entities;
        }

        public override byte PacketId
        {
            get { return 0x1D; }
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
            byte[] payload = new[] { PacketId, (byte)EntityIds.Length };
            foreach (int id in EntityIds)
                payload = payload.Concat(DataUtility.CreateInt32(id)).ToArray();
            client.SendData(payload);
        }
    }
}
