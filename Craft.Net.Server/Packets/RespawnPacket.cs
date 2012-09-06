using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class RespawnPacket : Packet
    {
        public Dimension Dimension { get; set; }
        public Difficulty Difficulty { get; set; }
        public GameMode GameMode { get; set; }
        public short WorldHeight { get; set; }
        public string LevelType { get; set; }

        public RespawnPacket()
        {
        }

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
            get { return 0x9; }
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
                .Concat(DataUtility.CreateInt32((int)Dimension))
                .Concat(new byte[] { (byte)Difficulty, (byte)GameMode })
                .Concat(DataUtility.CreateInt16(WorldHeight))
                .Concat(DataUtility.CreateString(server.GetClientWorld(client).LevelType))
                .ToArray();
            client.SendData(payload);
        }
    }
}
