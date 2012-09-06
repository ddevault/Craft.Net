using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class SpawnPositionPacket : Packet
    {
        public Vector3 SpawnPosition;

        public SpawnPositionPacket()
        {
        }

        public SpawnPositionPacket(Vector3 spawnPosition)
        {
            SpawnPosition = spawnPosition;
        }

        public override byte PacketId
        {
            get { return 0x6; }
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
                .Concat(DataUtility.CreateInt32((int)SpawnPosition.X))
                .Concat(DataUtility.CreateInt32((int)SpawnPosition.Y))
                .Concat(DataUtility.CreateInt32((int)SpawnPosition.Z))
                .ToArray();
            client.SendData(payload);
        }
    }
}
