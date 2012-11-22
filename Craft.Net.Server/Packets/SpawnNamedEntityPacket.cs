using System;
using Craft.Net.Data;
using Craft.Net.Data.Metadata;

namespace Craft.Net.Server.Packets
{
   public class SpawnNamedEntityPacket : Packet
   {
      public short CurrentItem;
      public int EntityId;
      public float Pitch;
      public string PlayerName;
      public Vector3 Position;
      public float Yaw;
      public MetadataDictionary Metadata;

      public SpawnNamedEntityPacket()
      {
      }

      public SpawnNamedEntityPacket(MinecraftClient client)
      {
         EntityId = client.Entity.Id;
         PlayerName = client.Username;
         Position = client.Entity.Position;
         Yaw = client.Entity.Yaw;
         Pitch = client.Entity.Pitch;
         CurrentItem = 0; // TODO
         Metadata = client.Entity.Metadata;
      }

      public override byte PacketId
      {
         get { return 0x14; }
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
            DataUtility.CreateString(PlayerName),
            DataUtility.CreateAbsoluteInteger((int)Position.X),
            DataUtility.CreateAbsoluteInteger((int)Position.Y),
            DataUtility.CreateAbsoluteInteger((int)Position.Z),
            DataUtility.CreatePackedByte(Pitch),
            DataUtility.CreatePackedByte(Yaw),
            DataUtility.CreateInt16(CurrentItem),
            Metadata.Encode()));
         }
      }
}