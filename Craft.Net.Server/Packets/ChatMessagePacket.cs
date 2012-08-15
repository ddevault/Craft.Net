using System.Linq;
using Craft.Net.Server.Events;

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
            get { return 0x3; }
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
            Server.Log("<" + Client.Username + "> " + Message);
            var args = new ChatMessageEventArgs(Client, Message);
            Server.FireOnChatMessage(args);
            if (!args.Handled)
                Server.SendChat("<" + Client.Username + "> " + Message);
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] buffer = new[] {PacketID}
                .Concat(CreateString(Message)).ToArray();
            Client.SendData(buffer);
        }
    }
}