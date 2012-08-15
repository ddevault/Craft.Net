namespace Craft.Net.Server.Packets
{
    public class CloseWindowPacket : Packet
    {
        public byte WindowId;

        public override byte PacketID
        {
            get { return 0x65; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadByte(buffer, ref offset, out WindowId))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            // Do nothing
            // TODO: Do something?
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            var buffer = new[] {PacketID, WindowId};
            client.SendData(buffer);
        }
    }
}