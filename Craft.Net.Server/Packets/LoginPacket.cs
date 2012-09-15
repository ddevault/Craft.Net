using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class LoginPacket : Packet
    {
        private Difficulty Difficulty;
        private Dimension Dimension;
        private int EntityId;
        private GameMode GameMode;
        private string LevelType;
        private byte MaxPlayers;

        public LoginPacket(int entityId, string levelType,
                           GameMode gameMode, Dimension dimension,
                           Difficulty difficulty, byte maxPlayers)
        {
            this.EntityId = entityId;
            this.LevelType = levelType;
            this.GameMode = gameMode;
            this.Dimension = dimension;
            this.Difficulty = difficulty;
            this.MaxPlayers = maxPlayers;
        }

        public override byte PacketId
        {
            get { return 0x01; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            return 1;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            // TODO: Send world, etc
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(
                DataUtility.CreateInt32(EntityId),
                DataUtility.CreateString(LevelType),
                new byte[]
                {
                    (byte)GameMode,
                    (byte)Dimension,
                    (byte)Difficulty,
                    0,
                    MaxPlayers
                }));
        }
    }
}