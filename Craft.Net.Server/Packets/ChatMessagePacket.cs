using System;
using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class ChatMessagePacket : Packet
    {
        public string Message;

        public ChatMessagePacket()
        {
        }

        public ChatMessagePacket(string Message)
        {
            this.Message = Message;
        }

        public override byte PacketID
        {
            get
            {
                return 0x3;
            }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 1;
            if (!TryReadString(Buffer, ref offset, out Message))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            Console.WriteLine(Message);
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new byte[] { PacketID }
                .Concat(CreateString(Message)).ToArray();
            Client.SendData(buffer);
        }
    }
}

