using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class RespawnPacket : Packet
    {
        private Dimension Dimension { get; set; }
        private Difficulty Difficulty { get; set; }
        private GameMode GameMode { get; set; }
        private short WorldHeight { get; set; }
        private string LevelType { get; set; }

        public RespawnPacket(Dimension dimension, Difficulty difficulty, GameMode gameMode,
            string levelType)
        {
            Dimension = dimension;
            Difficulty = difficulty;
            GameMode = gameMode;
            WorldHeight = 256;
            LevelType = levelType;
        }

        public override byte PacketId
        {
            get { return 0x09; }
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
                DataUtility.CreateInt32((int)Dimension),
                new[] 
                {
                   (byte)Difficulty,
                   (byte)GameMode,
                },
                DataUtility.CreateInt16(WorldHeight),
                DataUtility.CreateString(client.World.LevelType)));
        }
    }
}
