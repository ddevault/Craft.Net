using System;

namespace Craft.Net.Server.Packets
{
    public class PlayerAbilitiesPacket : Packet
    {
        public byte WalkingSpeed, FlyingSpeed;

        public PlayerAbilitiesPacket()
        {
        }

        public PlayerAbilitiesPacket(byte WalkingSpeed, byte FlyingSpeed)
        {
            this.WalkingSpeed = WalkingSpeed;
            this.FlyingSpeed = FlyingSpeed;
        }

        public override byte PacketID
        {
            get
            {
                return 0xCA;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            byte flags = 0;
            int offset = 1;
            if (!TryReadByte(Buffer, ref offset, out flags))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out WalkingSpeed))
                return -1;
            if (!TryReadByte(Buffer, ref offset, out FlyingSpeed))
                return -1;
            Console.WriteLine("0xCA Flags: 0x" + flags.ToString("x"));
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            // TODO
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            // TODO
        }
    }
}

