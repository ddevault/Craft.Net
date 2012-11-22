using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Events
{
   public class PacketEventArgs : EventArgs
   {
      public PacketEventArgs(Packet packet, MinecraftClient client, MinecraftServer server)
      {
         Packet = packet;
         Client = client;
         Server = server;
      }

      public PacketContext Context
      {
         get { return Packet.PacketContext; }
      }

      public Packet Packet { get; set; }
      public MinecraftClient Client { get; set; }
      public MinecraftServer Server { get; set; }
   }
}