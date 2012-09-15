using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
    public sealed class PluginMessagePacket : Packet
    {
        private string Channel;
        private byte[] Message;

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
            if (!DataUtility.TryReadInt16(buffer, ref offset, out messageLength))
                return -1;
            if (!DataUtility.TryReadArray(buffer, messageLength, ref offset, out Message))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, MinecraftClient client)
        {
            if (server.PluginChannels.ContainsKey(Channel))
                server.PluginChannels [Channel].MessageRecieved(client, Message);
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            client.SendData(CreateBuffer(
                DataUtility.CreateString(Channel),
                DataUtility.CreateInt16((short)Message.Length),
                Message));
        }
    }
}