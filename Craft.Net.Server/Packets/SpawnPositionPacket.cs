using System;
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
            get { return 0x06; }
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
                DataUtility.CreateInt32((int)SpawnPosition.X),
                DataUtility.CreateInt32((int)SpawnPosition.Y),
                DataUtility.CreateInt32((int)SpawnPosition.Z)));
        }
    }
}
