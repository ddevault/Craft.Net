using Craft.Net.Data;

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

      public override byte PacketId
      {
         get { return 0xFF; }
      }

      public override int TryReadPacket(byte[] buffer, int length)
      {
         int offset = 1;
         if (!DataUtility.TryReadString(buffer, ref offset, out Reason))
            return -1;
         return offset;
      }

      public override void HandlePacket(MinecraftServer server, MinecraftClient client)
      {
         LogProvider.Log(client.Username + " disconnected (" + Reason + ")");
         client.IsDisconnected = true;
      }

      public override void SendPacket(MinecraftServer server, MinecraftClient client)
      {
         if (!Reason.Contains("§"))
            LogProvider.Log("Disconnected client: " + Reason);

         client.SendData(CreateBuffer(DataUtility.CreateString(Reason)));
         client.IsDisconnected = true;
      }
   }
}