using System;

namespace Craft.Net.Server.Packets
{
    public class LocaleAndViewDistancePacket : Packet
    {
        public string Locale;
        public int ViewDistance;
        public bool ChatEnabled, ColorsEnabled;
        public Difficulty Difficulty;

        public LocaleAndViewDistancePacket()
        {
        }

        public override byte PacketID
        {
            get
            {
                return 0xCC;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            byte viewDistance = 0, chatFlags = 0, difficulty = 0;

            if (!TryReadString(Buffer, ref offset, out this.Locale))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out viewDistance))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out chatFlags))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out difficulty))
                return -1;

            // Adds an extra 2 chunk buffer to make loading look nice
            this.ViewDistance = 16 >> viewDistance + 2;

            this.ChatEnabled = (chatFlags & 0x1) == 0x1;
            this.ColorsEnabled = (chatFlags & 0x2) == 0x2;

            this.Difficulty = (Difficulty)difficulty;

            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Client.ChatEnabled = this.ChatEnabled;
            Client.ColorsEnabled = this.ColorsEnabled;
            Client.Locale = this.Locale;
            Client.ViewDistance = this.ViewDistance;
            // Difficulty is discarded
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            throw new System.NotImplementedException();
        }
    }
}

