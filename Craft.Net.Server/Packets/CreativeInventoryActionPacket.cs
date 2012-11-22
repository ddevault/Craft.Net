using System;
using Craft.Net.Data;

namespace Craft.Net.Server.Packets
{
   public class CreativeInventoryActionPacket : Packet
   {
      public short Index;
      public Slot Item;

      public override byte PacketId
      {
         get { return 0x6B; }
      }

      public override int TryReadPacket(byte[] buffer, int length)
      {
         int offset = 1;
         if (!DataUtility.TryReadInt16(buffer, ref offset, out Index))
            return -1;
         if (!Slot.TryReadSlot(buffer, ref offset, out Item))
            return -1;
         return offset;
      }

      public override void HandlePacket(MinecraftServer server, MinecraftClient client)
      {
         if (Index < client.Entity.Inventory.Length && Index > 0)
         {
            client.Entity.Inventory[Index] = Item;
            if (Index == client.Entity.SelectedSlot)
            {
               var clients = server.EntityManager.GetKnownClients(client.Entity);
               foreach (var _client in clients)
                  _client.SendPacket(new EntityEquipmentPacket(client.Entity.Id, EntityEquipmentSlot.HeldItem, client.Entity.Inventory[Index]));
               }
            }
         }

         public override void SendPacket(MinecraftServer server, MinecraftClient client)
         {
            throw new InvalidOperationException();
         }
      }
}