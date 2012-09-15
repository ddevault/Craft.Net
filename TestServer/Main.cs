using System;
using System.Globalization;
using Craft.Net.Server;
using System.Linq;
using System.Net;
using Craft.Net.Data;
using Craft.Net.Data.Generation;
using Craft.Net.Server.Channels;
using Craft.Net.Server.Events;
using System.Reflection;
using Craft.Net.Data.Blocks;
using System.IO;
using Craft.Net.Server.Packets;

namespace TestServer
{
    class MainClass
    {
        static MinecraftServer minecraftServer;

        public static void Main(string[] args)
        {
            // Create a server on 0.0.0.0:25565
            minecraftServer = new MinecraftServer(
		        new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.Settings.OnlineMode = false;
            minecraftServer.Settings.EnableEncryption = true;
            // Add a console logger
            LogProvider.RegisterProvider(new ConsoleLogWriter(LogImportance.Medium));
            LogProvider.RegisterProvider(new FileLogWriter("packetLog.txt", LogImportance.Low));
            // Add a flatland world
#if DEBUG
            // Use a fresh world each time
            if (Directory.Exists("world"))
                Directory.Delete("world", true);
#endif
            minecraftServer.AddLevel(new Level(Path.Combine(Directory.GetCurrentDirectory(), "world")));
            // Register the chat handler
            minecraftServer.ChatMessage += HandleOnChatMessage;
            // Start the server
            minecraftServer.Start();
            Console.WriteLine("Press any key to exit.");
            while (Console.ReadKey(true).Key != ConsoleKey.Q) { }
            // Stop the server
            minecraftServer.Stop();
            minecraftServer.DefaultLevel.Save();
        }

        static void HandleOnChatMessage(object sender, ChatMessageEventArgs e)
        {
            if (e.RawMessage.StartsWith("/"))
            {
                e.Handled = true;
                string command = e.RawMessage.Substring(1);
                if (command.Contains(" "))
                    command = command.Remove(command.IndexOf(' '));
                command = command.ToLower();
                switch (command)
                {
                    case "under":
                        try
                        {
                            e.Origin.SendChat("Block under you: " + 
                                e.Origin.World.GetBlock(e.Origin.Entity.Position + Vector3.Down).GetType().Name);
                        }
                        catch { }
                        break;
                    case "ping":
                        e.Origin.SendChat("Pong");
                        break;
                    case "lightning":
                        minecraftServer.WeatherManager.SpawnLightning(e.Origin.World, e.Origin.Entity.Position);
                        break;
                    case "velocity":
                        e.Origin.SendChat(e.Origin.Entity.Velocity.ToString());
                        break;
                    case "save":
                        minecraftServer.DefaultWorld.Regions[Vector3.Zero].Save();
                        break;
                    case "time":
                        var clients = minecraftServer.EntityManager.GetClientsInWorld(e.Origin.World);
                        minecraftServer.GetLevel(e.Origin.World).Time = 18000;
                        foreach (var minecraftClient in clients)
                            minecraftClient.SendPacket(new TimeUpdatePacket(18000));
                        break;
                    case "kill":
                        e.Origin.Entity.Health = 0;
                        break;
                    case "survival":
                        e.Origin.Entity.GameMode = GameMode.Survival;
                        break;
                    case "creative":
                        e.Origin.Entity.GameMode = GameMode.Creative;
                        break;
                    case "spawn":
                        // TODO: Why does this mess up stance
                        minecraftServer.EntityManager.TeleportEntity(e.Origin.Entity, e.Origin.Entity.SpawnPoint);
                        break;
                    case "jump":
                        e.Origin.Entity.Velocity = new Vector3(0, 10, 0);
                        break;
                    case "forward":
                        Vector3 velocity = DataUtility.RotateY(Vector3.Forwards * 5,
                            DataUtility.DegreesToRadians(e.Origin.Entity.Yaw));
                        //velocity.X = -velocity.X;
                        e.Origin.Entity.Velocity = velocity;
                        e.Origin.SendChat(e.Origin.Entity.Yaw.ToString(CultureInfo.InvariantCulture));
                        break;
                    case "flying":
                        e.Origin.Entity.Abilities.MayFly = !e.Origin.Entity.Abilities.MayFly;
                        break;
                    case "instantmine":
                        e.Origin.Entity.Abilities.InstantMine = !e.Origin.Entity.Abilities.InstantMine;
                        break;
                }
            }
        }
    }
}
