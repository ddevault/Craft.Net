using System;
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

namespace Craft.Net.Server.Test
{
    class MainClass
    {
        static MinecraftServer minecraftServer;

        public static void Main(string[] args)
        {
            // Create a server on 0.0.0.0:25565
            minecraftServer = new MinecraftServer(
		        new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.OnlineMode = false;
            minecraftServer.EncryptionEnabled = true;
            // Add a console logger
            minecraftServer.AddLogProvider(new ConsoleLogWriter(LogImportance.High));
            minecraftServer.AddLogProvider(new FileLogWriter("packetLog.txt", LogImportance.Low));
            // Add a flatland world
            minecraftServer.AddLevel(new Level(Path.Combine(Directory.GetCurrentDirectory(), "world")));
            minecraftServer.DefaultLevel.GameMode = GameMode.Creative;
            // Register the chat handler
            minecraftServer.ChatMessage += HandleOnChatMessage;
            // Start the server
            minecraftServer.Start();
            Console.WriteLine("Press any key to exit.");
            while (Console.ReadKey(true).Key != ConsoleKey.Q)
		        continue;
            // Stop the server
            minecraftServer.Stop();
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
                                minecraftServer.GetClientWorld(e.Origin).GetBlock(
                                e.Origin.Entity.Position + Vector3.Down).GetType().Name);
                        }
                        catch { }
                        break;
                    case "ping":
                        e.Origin.SendChat("Pong");
                        break;
                    case "lightning":
                        minecraftServer.SpawnLightning(minecraftServer.GetClientWorld(e.Origin), e.Origin.Entity.Position);
                        break;
                    case "velocity":
                        e.Origin.SendChat(e.Origin.Entity.Velocity.ToString());
                        break;
                    case "save":
                        minecraftServer.DefaultWorld.Regions[Vector3.Zero].Save();
                        break;
                    case "time":
                        var clients = minecraftServer.GetClientsInWorld(minecraftServer.GetClientWorld(e.Origin));
                        foreach (var minecraftClient in clients )
                            minecraftClient.SendPacket(new TimeUpdatePacket(18000));
                        break;
                }
            }
        }
    }
}
