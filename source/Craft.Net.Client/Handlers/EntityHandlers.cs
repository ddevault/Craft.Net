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
            if (Math.Abs(packet.X) < 0.01 && Math.Abs(packet.X) > 0)
                return; // Sometimes the vanilla server sends weird position updates like this
            client.Position = new Vector3(packet.X, packet.Y, packet.Z);
            if (!client.IsSpawned)
            {
                client.IsSpawned = true;
                client.OnInitialSpawn(new EntitySpawnEventArgs(client.Position, client.EntityId));
            }
            client.SendPacket(new PlayerPositionPacket(client.Position.X, client.Position.Y, client.Position.Z, client.Position.Y - 1.62, true));
        }

        public static void EntityTeleport(MinecraftClient client, IPacket _packet)
        {
            var packet = (EntityTeleportPacket)_packet;
            if (packet.EntityId == client.EntityId)
                client.Position = new Vector3(packet.X, packet.Y, packet.Z);
        }
    }
}