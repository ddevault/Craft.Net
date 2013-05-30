using Craft.Net.Entities;
using Craft.Net.Logic.Windows;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
    public class PlayerManager : IDisposable
    {
        public PlayerManager(RemoteClient client, MinecraftServer server)
        {
            Client = client;
            Server = server;
            client.Entity.PickUpItem += Entity_PickUpItem;
            client.Entity.Inventory.WindowChange += Inventory_WindowChange;
        }

        public MinecraftServer Server { get; set; }
        public RemoteClient Client { get; set; }

        private void Entity_PickUpItem(object sender, EntityEventArgs e)
        {
            var item = e.Entity as ItemEntity;
            var pickUp = Client.Entity.Inventory.PickUpStack(item.Item);
            if (pickUp)
            {
                var clients = Server.GetClientsInWorld(Client.World);
                foreach (var client in clients)
                    client.SendPacket(new CollectItemPacket(e.Entity.EntityId, Client.Entity.EntityId));
                Server.EntityManager.Despawn(e.Entity);
            }
        }

        private void Inventory_WindowChange(object sender, WindowChangeEventArgs e)
        {
            Client.SendPacket(new SetSlotPacket(0, (short)e.SlotIndex, e.Value));
        }

        public void Dispose()
        {
            Client.Entity.PickUpItem -= Entity_PickUpItem;
            Client.Entity.Inventory.WindowChange -= Inventory_WindowChange;
        }
    }
}
