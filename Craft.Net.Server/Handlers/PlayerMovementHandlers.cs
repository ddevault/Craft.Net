using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;

namespace Craft.Net.Server.Handlers
{
    internal static class PlayerMovementHandlers
    {
        public static void Player(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            // No action needed for this packet
        }

        public static void PlayerPosition(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var playerPosition = (PlayerPositionPacket)packet;
            client.Entity.FoodExhaustion += (float)client.Entity.GivenPosition.DistanceTo(
                new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z)) *
                (client.Entity.IsSprinting ? 0.1f : 0.01f); // TODO: Swimming

            if ((playerPosition.Y - client.Entity.GivenPosition.Y) > 0)
                client.Entity.PositiveDeltaY += (playerPosition.Y - client.Entity.GivenPosition.Y);
            else
                client.Entity.PositiveDeltaY = 0;

            client.Entity.GivenPosition = new Vector3(playerPosition.X, playerPosition.Y, playerPosition.Z);
            client.UpdateChunksAsync();
        }

        public static void PlayerLook(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var playerLook = (PlayerLookPacket)packet;
            client.Entity.Pitch = playerLook.Pitch;
            client.Entity.Yaw = playerLook.Yaw;
        }

        public static void PlayerPositionAndLook(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var positionAndLook = (PlayerPositionAndLookPacket)packet;
            // Position
            client.Entity.FoodExhaustion += (float)client.Entity.GivenPosition.DistanceTo(
                new Vector3(positionAndLook.X, positionAndLook.Y, positionAndLook.Z)) *
                (client.Entity.IsSprinting ? 0.1f : 0.01f); // TODO: Swimming

            if ((positionAndLook.Y - client.Entity.GivenPosition.Y) > 0)
                client.Entity.PositiveDeltaY += (positionAndLook.Y - client.Entity.GivenPosition.Y);
            else
                client.Entity.PositiveDeltaY = 0;

            client.Entity.GivenPosition = new Vector3(positionAndLook.X, positionAndLook.Y, positionAndLook.Z);
            client.UpdateChunksAsync();
            // Look
            client.Entity.Pitch = positionAndLook.Pitch;
            client.Entity.Yaw = positionAndLook.Yaw;
        }

        public static void Animation(MinecraftClient client, MinecraftServer server, IPacket packet)
        {
            var animation = (AnimationPacket)packet;
            var clients = server.EntityManager.GetKnownClients(client.Entity);
            foreach (var _client in clients)
                _client.SendPacket(animation);
        }
    }
}
