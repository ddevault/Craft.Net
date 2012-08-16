using System.Linq;
using Craft.Net.Data;

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

        public override byte PacketId
        {
            get { return 0xFA; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 0;
            short messageLength = 0;
            if (!DataUtility.TryReadString(buffer, ref offset, out Channel))
                return -1;
            if (!DataUtility.TryReadShort(buffer, ref offset, out messageLength))
                return -1;
            if (!DataUtility.TryReadArray(buffer, messageLength, ref offset, out Message))
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
            byte[] data = new[] {PacketId}
                .Concat(DataUtility.CreateString(Channel))
                .Concat(DataUtility.CreateShort((short)Message.Length))
                .Concat(Message).ToArray();
            client.SendData(data);
        }
    }
}