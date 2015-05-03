using Craft.Net.Logic;
using Craft.Net.Logic.Windows;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Craft.Net.Common;

namespace Craft.Net.Server
{
    public class PlayerManager : IDisposable
    {
        public PlayerManager(RemoteClient client, MinecraftServer server)
        {
            Client = client;
            Server = server;
            SendInventoryUpdates = true;
            client.Entity.PickUpItem += Entity_PickUpItem;
            client.Entity.InteractBlock += Interact_Block;
            client.Entity.Inventory.WindowChange += Inventory_WindowChange;
            client.PropertyChanged += client_PropertyChanged;
        }

        public MinecraftServer Server { get; set; }
        public RemoteClient Client { get; set; }
        internal bool SendInventoryUpdates { get; set; }

        private void Interact_Block(object sender, InteractionEventArgs e)
        {
            //TODO : Find a way to do multiple intration block here EAT Craft Boat..
            //For now its hard implement a load from save here load item for save in the workbech if item in it...
            ItemStack[] items = new ItemStack[]
            {
                ItemStack.EmptyStack,//output
                ItemStack.EmptyStack,ItemStack.EmptyStack,ItemStack.EmptyStack,
                ItemStack.EmptyStack,ItemStack.EmptyStack,ItemStack.EmptyStack,
                ItemStack.EmptyStack,ItemStack.EmptyStack,ItemStack.EmptyStack
            };
            
            WorkBenchWindow window = new WorkBenchWindow();
            window.LoadWorkBenchItem(items);
            //load the windows item from the player inventory
            window.WindowAreas[1].Items = Client.Entity.Inventory.MainInventory.Items;
            window.WindowAreas[2].Items = Client.Entity.Inventory.Hotbar.Items;
            Client.Entity.WorkBench = window;
            Client.Entity.WorkBench.WindowChange += WorkBench_WindowChange;

            Client.SendPacket(new OpenWindowPacket(window.Id, "minecraft:crafting_table", "Crafting", 0, null));
            Client.SendPacket(new SetWindowItemsPacket(window.Id, items));
        }

        private void WorkBench_WindowChange(object sender, WindowChangeEventArgs e)
        {
            if (SendInventoryUpdates)
                Client.SendPacket(new SetSlotPacket(120, (short)e.SlotIndex, e.Value));
            //Implement a way to work whit crafting recipe
        }

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
            if (SendInventoryUpdates)
                Client.SendPacket(new SetSlotPacket(0, (short)e.SlotIndex, e.Value));
        }

        void client_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "GameMode":
                    Client.SendPacket(new ChangeGameStatePacket(ChangeGameStatePacket.GameState.ChangeGameMode, (float)Client.GameMode));
                    if (Client.GameMode == GameMode.Creative)
                    {
                        Client.Entity.Abilities.InstantMine = true;
                        Client.Entity.Abilities.MayFly = true;
                    }
                    else
                    {
                        Client.Entity.Abilities.InstantMine = false;
                        Client.Entity.Abilities.MayFly = false;
                    }
                    break;
            }
        }

        public void Dispose()
        {
            Client.Entity.PickUpItem -= Entity_PickUpItem;
            Client.Entity.Inventory.WindowChange -= Inventory_WindowChange;
            Client.PropertyChanged -= client_PropertyChanged;
        }
    }
}
