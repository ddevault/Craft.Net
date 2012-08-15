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

        public PluginMessagePacket(string channel, byte[] message)
        {
            this.Channel = channel;
            this.Message = message;
        }

        public override byte PacketID
        {
            get { return 0xFA; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 0;
            short messageLength = 0;
            if (!TryReadString(buffer, ref offset, out Channel))
                return -1;
            if (!TryReadShort(buffer, ref offset, out messageLength))
                return -1;
            if (!TryReadArray(buffer, messageLength, ref offset, out Message))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            if (server.PluginChannels.ContainsKey(Channel))
                server.PluginChannels[Channel].MessageRecieved(Message);
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            byte[] data = new[] {PacketID}
                .Concat(CreateString(Channel))
                .Concat(CreateShort((short)Message.Length))
                .Concat(Message).ToArray();
            client.SendData(data);
        }
    }
}