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
        public enum WeatherState
        {
            Clear,
            Raining,
            Thundering
        }

        public MinecraftServer Server { get; set; }
        public World World { get; set; }
        public DateTime NextRainChange { get; set; }
        public DateTime NextThunderChange { get; set; }
        public WeatherState CurrentWeatherState { get; set; }
        // TODO: If there is a way to do this without the private field, change this.
        public bool IsRainActive { get { return isRainActive; } set { isRainActive = value; DoWeatherChange(); } }
        public bool IsThunderActive { get { return isThunderActive; } set { isThunderActive = value; DoWeatherChange(); } } 

        private bool isRainActive;
        private bool isThunderActive;
        private int weatherTick;
        private Random random;

        public WeatherManager(World world, MinecraftServer server)
        {
            Server = server;
            World = world;
            random = new Random();
            ScheduleChange(true);
            ScheduleChange(false);
        }

        /// <summary>
        /// Schedules the next change in weather.
        /// </summary>
        /// <param name="thunder">
        /// Whether the weather change should be
        /// thunder or rain
        /// </param>
        public void ScheduleChange(bool thunder)
        {
            // TODO: Fine tune weather changes.
            if (!thunder)
                NextRainChange = DateTime.Now + TimeSpan.FromSeconds(random.Next(20) + 10);
            else
                NextThunderChange = DateTime.Now + TimeSpan.FromSeconds(random.Next(20) + 10);
        }

        /// <summary>
        /// Performs a weather update
        /// </summary>
        public void DoWeatherUpdate()
        {
            // Weather doesn't need to take every tick.
            if (weatherTick < 99)
            {
                weatherTick++;
                return;
            }
            else
                weatherTick = 0;

            // Update level info
            DoLevelUpdate();

            // Update weather
            if (NextRainChange <= DateTime.Now)
            {
                isRainActive = !isRainActive;
                DoWeatherChange();
            }
            if (NextThunderChange <= DateTime.Now)
            {
                isThunderActive = !isThunderActive;
                ScheduleChange(true);
            }

            // Do weather effects
            if (isRainActive)
            {
                DoWeatherSnow();
                if (isThunderActive && random.Next(4) == 0)
                    DoWeatherThunder();
            }
        }

        /// <summary>
        /// Performs a change in weather.
        /// </summary>
        public void DoWeatherChange()
        {
            var clients = Server.EntityManager.GetClientsInWorld(World);
            if (!isRainActive)
                CurrentWeatherState = WeatherState.Clear;
            else if (isRainActive && isThunderActive)
                CurrentWeatherState = WeatherState.Thundering;
            else
                 CurrentWeatherState = WeatherState.Raining;
            
            if (isRainActive)
            {
                foreach (var minecraftClient in clients)
                    SendWeatherToClient(WeatherState.Raining, minecraftClient);
            }
            else
            {
                foreach (var minecraftClient in clients)
                    SendWeatherToClient(WeatherState.Clear, minecraftClient);
            }
            // TODO: Lighting.
            ScheduleChange(false);
        }

        /// <summary>
        /// Sends the weather currently in effect to the 
        /// client.
        /// </summary>
        public void SendCurrentWeatherToClient(MinecraftClient client)
        {
            if(isRainActive)
                SendWeatherToClient(CurrentWeatherState, client);
        }

        public void SendWeatherToClient(WeatherState state, MinecraftClient client)
        {
            if (client.World != World)
                throw new InvalidOperationException("Client must be in same world to change WeatherState!");

            switch (state)
            {
                case WeatherState.Clear:
                    client.SendPacket(new ChangeGameStatePacket(ChangeGameStatePacket.GameState.EndRaining));
                    break;
                case WeatherState.Raining:
                case WeatherState.Thundering:
                    client.SendPacket(new ChangeGameStatePacket(ChangeGameStatePacket.GameState.BeginRaining));
                    break;
            }
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
            // TODO: Visually, lightning bolt will always spawn at 0,0.  Fix this.
            foreach (var minecraftClient in clients)
                minecraftClient.SendPacket(new SpawnGlobalEntityPacket(EntityManager.nextEntityId++, 1,
                    (int)strike.X, (int)strike.Y, (int)strike.Z));
        }

        protected virtual void DoWeatherThunder()
        {
            // TODO: Optimize
            var clients = Server.EntityManager.GetClientsInWorld(World);
            var chunksDone = new List<Vector3>();
            foreach (var client in clients.ToArray())
            {
                foreach (var chunkLocation in client.LoadedChunks.ToArray())
                {
                    if (chunksDone.Contains(chunkLocation))
                        continue;

                    var chunk = World.GetChunk(chunkLocation);
                    if (random.Next(10) == 0)
                    {
                        var location = new Vector3(random.Next(16), 0, random.Next(16)) + chunk.AbsolutePosition;
                        SpawnLightning(location);
                        return;
                    }
                }
            }
        }

        protected virtual void DoWeatherSnow()
        {
            // TODO: Optimize
            var clients = Server.EntityManager.GetClientsInWorld(World);
            var chunksDone = new List<Vector3>();
            foreach (var client in clients.ToArray())
            {
                foreach (var chunkLocation in client.LoadedChunks.ToArray())
                {
                    // Only do snow update once per chunk.
                    if (chunksDone.Contains(chunkLocation))
                        continue;

                    var chunk = World.GetChunk(chunkLocation);
                    // Begin problem section
                    for (byte x = 0; x < 16; x++)
                    {
                        for (byte z = 0; z < 16; z++)
                        {
                            if ((chunk.GetBiome(x, z) == Biome.IceMountains || chunk.GetBiome(x, z) == Biome.IcePlains || chunk.GetBiome(x, z) == Biome.Taiga || chunk.GetBiome(x, z) == Biome.TaigaHills) && random.Next(58) == 0)
                            {
                                var block = chunk.GetTopBlock(x, z);
                                // I'll just leave this here.  It might be needed someday.
                                //if (block is SnowfallBlock)
                                //{
                                //    if (block.Data < 7)
                                //    {
                                //        block.Data++;
                                //        World.SetBlock(new Vector3(x, chunk.GetHeight(x, z), z) + chunk.AbsolutePosition, block);
                                //    }
                                //}
                                if (block.Transparency == Transparency.Opaque && !(block is SnowfallBlock))
                                    World.SetBlock(new Vector3(x, chunk.GetHeight(x, z) + 1, z) + chunk.AbsolutePosition, new SnowfallBlock());
                            }
                        }
                    }
                    // End problem section
                }
            }
        }

        private void DoLevelUpdate()
        {
            var rain = (int)((DateTime.Now - NextRainChange).TotalSeconds * 20) < 0 ? Math.Abs((int)((DateTime.Now - NextRainChange).TotalSeconds * 20)) : 0;
            var thunder = (int)((DateTime.Now - NextThunderChange).TotalSeconds * 20) < 0 ? Math.Abs((int)((DateTime.Now - NextThunderChange).TotalSeconds * 20)) : 0;
            // TODO: Is this wrong?  Should it be the exact tick that weather should change?
            World.Level.RainTime = rain;
            World.Level.ThunderTime = thunder;
            World.Level.Raining = isRainActive;
            World.Level.Thundering = isThunderActive;
        }
    }
}
