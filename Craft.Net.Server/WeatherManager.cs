using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Data;
using Craft.Net.Data.Blocks;

namespace Craft.Net.Server
{
    public class WeatherManager
    {
        public MinecraftServer Server { get; set; }
        public World World { get; set; }
        public DateTime NextWeatherChange { get; set; }

        public WeatherManager(World world, MinecraftServer server)
        {
            Server = server;
            World = world;
        }

        public void ScheduleChange()
        {
            
        }

        /// <summary>
        /// Spawns a bolt of lightning in the given world
        /// at the given position.
        /// </summary>
        public void SpawnLightning(Vector3 position)
        {
            var chunk = World.GetChunk(World.WorldToChunkCoordinates(position));
            var block = World.FindBlockPosition(position);
            int y = chunk.GetHeight((byte)block.X, (byte)block.Z) + 1;

            var strike = new Vector3(position.X, y, position.Z);
            if (World.GetBlock(strike + Vector3.Down).Transparency == Transparency.Opaque)
                World.SetBlock(strike, new FireBlock());

            var clients = Server.EntityManager.GetClientsInWorld(World);
            foreach (var minecraftClient in clients)
                minecraftClient.SendPacket(new SpawnGlobalEntityPacket(EntityManager.nextEntityId++, 1,
                    (int)strike.X, (int)strike.Y, (int)strike.Z));
        }
    }
}
