using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Client.Events;
using Craft.Net.Networking;
using Craft.Net.Common;

namespace Craft.Net.Client.Handlers
{
    internal static class EntityHandlers
    {
        public static void PlayerPositionAndLook(MinecraftClient client, IPacket _packet)
        {
            var packet = (PlayerPositionAndLookPacket)_packet;
            client._position = new Vector3(packet.X, packet.Y, packet.Z);
            if (!client.IsSpawned)
            {
                client.IsSpawned = true;
                client.OnInitialSpawn(new EntitySpawnEventArgs(client.Position, client.EntityId));
            }

            client.SendPacket(new PlayerPositionPacket(packet.X, packet.Y + 1.6 /* TODO Player.Height or something */, packet.Z, packet.Y, true));
        }

        public static void EntityTeleport(MinecraftClient client, IPacket _packet)
        {
            var packet = (EntityTeleportPacket)_packet;
            if (packet.EntityId == client.EntityId)
                client._position = new Vector3(packet.X, packet.Y, packet.Z);
        }
    }
}