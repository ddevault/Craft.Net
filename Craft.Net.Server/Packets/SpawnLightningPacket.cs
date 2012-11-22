using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
   public class SpawnLightningPacket : Packet
   {
      public int EntityId;
      public Vector3 Position;

      public SpawnLightningPacket()
      {
      }

      public SpawnLightningPacket(int entityId, Vector3 position)
      {
         EntityId = entityId;
         Position = position;
      }

      public override byte PacketId
      {
         get { return 0x47; }
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
            DataUtility.CreateInt32(EntityId),
            DataUtility.CreateBoolean(true),
            DataUtility.CreateAbsoluteInteger(Position.X),
            DataUtility.CreateAbsoluteInteger(Position.Y),
            DataUtility.CreateAbsoluteInteger(Position.Z)));
         }
      }
}