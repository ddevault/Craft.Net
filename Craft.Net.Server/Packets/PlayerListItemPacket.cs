using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
   public class PlayerListItemPacket : Packet
   {
      public bool Online;
      public short Ping;
      public string PlayerName;

      public PlayerListItemPacket()
      {
      }

      public PlayerListItemPacket(string playerName, bool online, short ping)
      {
         this.PlayerName = playerName;
         this.Online = online;
         this.Ping = ping;
      }

      public override byte PacketId
      {
         get { return 0xC9; }
      }

      public override int TryReadPacket(byte[] buffer, int length)
      {
         throw new InvalidOperationException();
      }

      public override void HandlePacket(MinecraftServer server, MinecraftClient client)
      {
         throw new InvalidOperationException();
      }

      public override void SendPacket(MinecraftServer server, MinecraftClient client)
      {
         client.SendData(CreateBuffer(
            DataUtility.CreateString(PlayerName),
            DataUtility.CreateBoolean(Online),
            DataUtility.CreateInt16(Ping)));
         }
      }
}