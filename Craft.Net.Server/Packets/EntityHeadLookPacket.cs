using System;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
   public class EntityHeadLookPacket : Packet
   {
      public int EntityId;
      public byte HeadYaw;

      public EntityHeadLookPacket()
      {
      }

      public EntityHeadLookPacket(Entity entity)
      {
         EntityId = entity.Id;
         HeadYaw = DataUtility.CreatePackedByte(entity.Yaw) [0];
      }

      public override byte PacketId
      {
         get { return 0x23; }
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
            new[] {HeadYaw}));
         }
      }
}