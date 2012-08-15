namespace Craft.Net.Server.Packets
{
    public class CloseWindowPacket : Packet
    {
        public byte WindowId;

        public override byte PacketID
        {
            get { return 0x65; }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadByte(Buffer, ref offset, out WindowId))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            // Do nothing
            // TODO: Do something?
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            var buffer = new[] {PacketID, WindowId};
            Client.SendData(buffer);
        }
    }
}