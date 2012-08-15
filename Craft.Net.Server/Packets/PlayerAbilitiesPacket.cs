namespace Craft.Net.Server.Packets
{
    public class PlayerAbilitiesPacket : Packet
    {
        public byte FlyingSpeed;
        public byte WalkingSpeed;

        public PlayerAbilitiesPacket()
        {
        }

        public PlayerAbilitiesPacket(byte walkingSpeed, byte flyingSpeed)
        {
            this.WalkingSpeed = walkingSpeed;
            this.FlyingSpeed = flyingSpeed;
        }

        public override byte PacketID
        {
            get { return 0xCA; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            byte flags = 0;
            int offset = 1;
            if (!TryReadByte(buffer, ref offset, out flags))
                return -1;
            if (!TryReadByte(buffer, ref offset, out WalkingSpeed))
                return -1;
            if (!TryReadByte(buffer, ref offset, out FlyingSpeed))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            // TODO
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            // TODO
        }
    }
}