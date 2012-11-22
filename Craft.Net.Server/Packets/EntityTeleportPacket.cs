using System;
using Craft.Net.Data;
using Craft.Net.Data.Entities;

namespace Craft.Net.Server.Packets
{
   public class EntityTeleportPacket : Packet
   {
      public int EntityId;
      public Vector3 Position;
      public float Yaw, Pitch;

      public EntityTeleportPacket()
      {
      }

      public EntityTeleportPacket(Entity entity)
      {
         EntityId = entity.Id;
         Position = entity.Position;
         this.Yaw = entity.Yaw;
         this.Pitch = entity.Pitch;
      }

      public override byte PacketId
      {
         get { return 0x22; }
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
            DataUtility.CreateAbsoluteInteger(Position.X),
            DataUtility.CreateAbsoluteInteger(Position.Y),
            DataUtility.CreateAbsoluteInteger(Position.Z),
            DataUtility.CreatePackedByte(Yaw),
            DataUtility.CreatePackedByte(Pitch)));
         }
      }
}