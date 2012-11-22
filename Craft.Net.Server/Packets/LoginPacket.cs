using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public class LoginPacket : Packet
    {
        public Difficulty Difficulty;
        public Dimension Dimension;
        public int EntityId;
        public GameMode GameMode;
        public string LevelType;
        public byte MaxPlayers;

        public LoginPacket()
        {
        }

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