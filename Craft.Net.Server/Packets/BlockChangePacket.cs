using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
   public class BlockChangePacket : Packet
   {
      public Vector3 Position;
      public Block Value;

      public BlockChangePacket()
      {
      }

      public BlockChangePacket(Vector3 position, Block value)
      {
         this.Position = position;
         this.Value = value;
      }

      public override byte PacketId
      {
         get { return 0x35; }
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
            DataUtility.CreateInt32((int)Position.X),
            new[] {(byte)Position.Y},
            DataUtility.CreateInt32((int)Position.Z),
            DataUtility.CreateUInt16(Value.Id),
            new[] {Value.Metadata}));
         }
      }
}