using Craft.Net.Common;
using Craft.Net.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Handlers
{
    internal static class PlayerMovementHandlers
    {
        public static void Player(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            // No action needed for this packet
        }

        public static void PlayerPosition(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerPositionPacket)_packet;
            client.Entity.FoodExhaustion += (float)client.Entity.Position.DistanceTo(
                new Vector3(packet.X, packet.Y, packet.Z));// *
                //(client.Entity.IsSprinting ? 0.1f : 0.01f); // TODO: Swimming

            if ((packet.Y - client.Entity.Position.Y) > 0)
                client.Entity.PositiveDeltaY += (packet.Y - client.Entity.Position.Y);
            else
                client.Entity.PositiveDeltaY = 0;

            client.Entity.Position = new Vector3(packet.X, packet.Y, packet.Z);
            client.UpdateChunksAsync();
        }

        public static void PlayerLook(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerLookPacket)_packet;
            client.Entity.Pitch = packet.Pitch;
            client.Entity.Yaw = packet.Yaw;
            client.Entity.HeadYaw = packet.Yaw;
        }

        public static void PlayerPositionAndLook(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerPositionAndLookPacket)_packet;
            // Position
            client.Entity.FoodExhaustion += (float)client.Entity.Position.DistanceTo(
                new Vector3(packet.X, packet.Y, packet.Z));// *
                //(client.Entity.IsSprinting ? 0.1f : 0.01f); // TODO: Swimming

            if ((packet.Y - client.Entity.Position.Y) > 0)
                client.Entity.PositiveDeltaY += (packet.Y - client.Entity.Position.Y);
            else
                client.Entity.PositiveDeltaY = 0;

            client.Entity.Position = new Vector3(packet.X, packet.Y, packet.Z);
            client.UpdateChunksAsync();
            // Look
            client.Entity.Pitch = packet.Pitch;
            client.Entity.Yaw = packet.Yaw;
            client.Entity.HeadYaw = packet.Yaw;
        }

        public static void Animation(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (AnimationPacket)_packet;
            var clients = server.EntityManager.GetKnownClients(client.Entity);
            foreach (var _client in clients)
                _client.SendPacket(packet);
        }

        public static void EntityAction(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (EntityActionPacket)_packet;
            switch (packet.Action)
            {
                case EntityActionPacket.EntityAction.Crouch:
                    client.Entity.IsCrouching = true;
                    break;
                case EntityActionPacket.EntityAction.Uncrouch:
                    client.Entity.IsCrouching = false;
                    break;
                case EntityActionPacket.EntityAction.StartSprinting:
                    client.Entity.IsSprinting = true;
                    break;
                case EntityActionPacket.EntityAction.StopSprinting:
                    client.Entity.IsSprinting = false;
                    break;
//                case EntityActionPacket.EntityAction.LeaveBed:
//                    client.Entity.LeaveBed();
//                    break;
            }
            if (packet.Action != EntityActionPacket.EntityAction.LeaveBed) // NOTE: Does this matter?
            {
                // TODO ?
            }
        }

        public static void PlayerAbilities(RemoteClient client, MinecraftServer server, IPacket _packet)
        {
            var packet = (PlayerAbilitiesPacket)_packet;
            if (client.GameMode == GameMode.Creative)
                client.Entity.Abilities.IsFlying = (packet.Flags & 2) == 2; // TODO: Make this packet more friendly
        }
    }
}
