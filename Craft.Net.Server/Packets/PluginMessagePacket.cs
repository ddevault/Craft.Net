using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class PluginMessagePacket : Packet
    {
        public string Channel;
        public byte[] Message;

        public PluginMessagePacket()
        {
        }

        public PluginMessagePacket(string Channel, byte[] Message)
        {
            this.Channel = Channel;
            this.Message = Message;
        }

        public override byte PacketID
        {
            get { return 0xFA; }
        }

        public override int TryReadPacket(byte[] Buffer, int Length)
        {
            int offset = 0;
            short length = 0;
            if (!TryReadString(Buffer, ref offset, out Channel))
                return -1;
            if (!TryReadShort(Buffer, ref offset, out length))
                return -1;
            if (!TryReadArray(Buffer, length, ref offset, out Message))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer Server, ref MinecraftClient Client)
        {
            if (Server.PluginChannels.ContainsKey(Channel))
                Server.PluginChannels[Channel].MessageRecieved(Message);
        }

        public override void SendPacket(MinecraftServer Server, MinecraftClient Client)
        {
            byte[] data = new[] {PacketID}
                .Concat(CreateString(Channel))
                .Concat(CreateShort((short) Message.Length))
                .Concat(Message).ToArray();
            Client.SendData(data);
        }
    }
}