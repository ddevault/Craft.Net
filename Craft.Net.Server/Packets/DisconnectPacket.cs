using System.Linq;

namespace Craft.Net.Server.Packets
{
    public class DisconnectPacket : Packet
    {
        public string Reason;

        public DisconnectPacket()
        {
        }

        public DisconnectPacket(string reason)
        {
            this.Reason = reason;
        }

        public override byte PacketID
        {
            get { return 0xFF; }
        }

        public override int TryReadPacket(byte[] buffer, int length)
        {
            int offset = 1;
            if (!TryReadString(buffer, ref offset, out Reason))
                return -1;
            return offset;
        }

        public override void HandlePacket(MinecraftServer server, ref MinecraftClient client)
        {
            server.Log(client.Username + " disconnected (" + Reason + ")");
            client.IsDisconnected = true;
        }

        public override void SendPacket(MinecraftServer server, MinecraftClient client)
        {
            if (!Reason.Contains("§"))
                server.Log("Disconnected client: " + Reason);
            byte[] buffer = new[] {PacketID}.Concat(CreateString(Reason)).ToArray();
            client.SendData(buffer);
            client.IsDisconnected = true;
        }
    }
}