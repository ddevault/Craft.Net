using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public enum ChatMode
    {
        Hidden = 2,
        CommandsOnly = 1,
        Enabled = 0
    }

    public class LocaleAndViewDistancePacket : Packet
    {
        public ChatMode ChatMode;
        public bool ColorsEnabled;
        public Difficulty Difficulty;
        public string Locale;
        public int ViewDistance;

        public override byte PacketId
        {
            get { return 0xCC; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            byte viewDistance = 0, chatFlags = 0, difficulty = 0;

            if (!DataUtility.TryReadString(buffer, ref offset, out Locale))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out viewDistance))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out chatFlags))
                return -1;
            if (!DataUtility.TryReadByte(buffer, ref offset, out difficulty))
                return -1;

            // Adds an extra 2 chunk buffer to make loading look nice
            ViewDistance = (8 << viewDistance) + 2;
            ChatMode = (ChatMode)(chatFlags & 0x3);
            ColorsEnabled = (chatFlags & 0x8) == 0x8;
            Difficulty = (Difficulty)difficulty;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            client.ChatMode = ChatMode;
            client.Locale = Locale;
            client.MaxViewDistance = ViewDistance;
            client.ColorsEnabled = ColorsEnabled;
            // Difficulty is discarded
            client.ForceUpdateChunksAsync();
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            throw new InvalidOperationException();
        }
    }
}