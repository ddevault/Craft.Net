using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Client.Events;
using Craft.Net.Networking;

namespace Craft.Net.Client.Handlers
{
    internal static class StateHandlers
    {
        public static void UpdateHealth(MinecraftClient client, IPacket _packet)
        {
            var packet = (UpdateHealthPacket)_packet;
            var eventArgs = new HealthAndFoodEventArgs(
                client.Health, client.Food, client.FoodSaturation);
            client.Health = packet.Health;
            client.Food = packet.Food;
            client.FoodSaturation = packet.FoodSaturation;
            eventArgs.Health = client.Health;
            eventArgs.Food = client.Food;
            eventArgs.FoodSaturation = client.FoodSaturation;
            if (client.Health <= 0)
                client.OnPlayerDied();
            if (eventArgs.IsChanged())
                client.OnHealthOrFoodChanged(eventArgs);
        }

        public static void Respawn(MinecraftClient client, IPacket _packet)
        {
            // TODO: Dimension change
            client.IsSpawned = false;
        }
    }
}