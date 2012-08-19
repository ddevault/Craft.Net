using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

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

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            throw new InvalidOperationException();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] payload = new byte[] { PacketId, (byte)EntityIds.Length };
            foreach (int id in EntityIds)
                payload = payload.Concat(DataUtility.CreateInt32(id)).ToArray();
            client.SendData(payload);
        }
    }
}
