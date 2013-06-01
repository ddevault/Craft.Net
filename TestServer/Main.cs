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
using Craft.Net;
using Craft.Net.Data.Windows;
using Craft.Net.Data.Items;

namespace TestServer
{
    class MainClass
    {
        static MinecraftServer minecraftServer;
        static String[] ops;

        public static void Main(string[] args)
        {
            // Create a server on 0.0.0.0:25565
            minecraftServer = new MinecraftServer(
                new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.Settings.OnlineMode = false;
            minecraftServer.Settings.EnableEncryption = true;
            CustomLeatherItem.Server = minecraftServer;
            Item.SetItemClass(new CustomLeatherItem());
            // Add a console logger
            LogProvider.RegisterProvider(new ConsoleLogWriter(LogImportance.Medium));
            LogProvider.RegisterProvider(new FileLogWriter("packetLog.txt", LogImportance.Low));
            // Add a flatland world
            IWorldGenerator generator = new FlatlandGenerator();
            minecraftServer.AddLevel(new Level(generator, Path.Combine(Directory.GetCurrentDirectory(), "world")));
            minecraftServer.DefaultLevel.GameMode = GameMode.Survival;
            // Register the chat handler
            minecraftServer.ChatMessage += HandleOnChatMessage;
            // Read Ops list
            if (File.Exists("ops.txt"))
            {
                StreamReader s = new System.IO.StreamReader("ops.txt");
                ops = s.ReadToEnd().Split(",".ToCharArray());
                s.Close();
            }
            else
            {
                StreamWriter s = new StreamWriter("ops.txt");
                ops = "".Split(",".ToCharArray());
                s.Close();
            }
            // Start the server
            minecraftServer.Start();
            Console.WriteLine("Press 'q' key to exit.");
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
                if (ops.Contains<String>(e.Origin.Username))
                {
                    string command = e.RawMessage.Substring(1);
                    string[] parameters = null;
                    if (command.Contains(" "))
                    {
                        parameters = command.Substring(command.IndexOf(' ') + 1).Split(' ');
                        command = command.Remove(command.IndexOf(' '));
                    }
                    command = command.ToLower();
                    switch (command)
                    {
                        case "ping":
                            e.Origin.SendChat("Pong");
                            break;
                        case "save":
                            minecraftServer.DefaultWorld.Regions[Vector3.Zero].Save();
                            break;
                        case "time":
                            if (parameters.Length == 2)
                            {
                                var clients = minecraftServer.EntityManager.GetClientsInWorld(e.Origin.World);
                                minecraftServer.GetLevel(e.Origin.World).Time = long.Parse(parameters[1]);
                                foreach (var minecraftClient in clients)
                                    minecraftClient.SendPacket(new TimeUpdatePacket(long.Parse(parameters[1]), long.Parse(parameters[1])));
                            }
                            else
                            {
                                e.Origin.SendChat("Usage: /time set [time]");
                            }
                            break;
                        case "kill":
                            e.Origin.Entity.Health = 0;
                            break;
                        case "spawn":
                            // TODO: Why does this mess up stance
                            minecraftServer.EntityManager.TeleportEntity(e.Origin.Entity, e.Origin.Entity.SpawnPoint);
                            break;
                        case "hunger":
                            if (parameters.Length == 1)
                            {
                                e.Origin.Entity.Food = short.Parse(parameters[0]);
                            }
                            else
                            {
                                e.Origin.SendChat("Usage: /hunger [value]");
                            }
                            break;
                        case "damage":
                            if (parameters.Length == 1)
                            {
                                e.Origin.Entity.Health = short.Parse(parameters[0]);
                            }
                            else
                            {
                                e.Origin.SendChat("Usage: /damage [value]");
                            }
                            break;
                        case "give":
                            try
                            {
                                var type = ((Block)int.Parse(parameters[0])).GetType();
                                var item = (Item)Activator.CreateInstance(type);
                                e.Origin.Entity.SetSlot(InventoryWindow.HotbarIndex, new ItemStack(item.Id, 1));
                            }
                            catch { e.Origin.SendChat("Usage: /give [id]"); }
                            break;                      
                         case "relight":
                            e.Origin.World.Relight();
                            e.Origin.SendChat("World relit.");
                            break;
                        
                        case "scoreboard":

                            try
                            {
                                switch (parameters[0])
                                {
                                    case "add":
                                        var board = minecraftServer.ScoreboardManager.CreateScoreboard(parameters[1], parameters[2]);
                                        minecraftServer.ScoreboardManager.DisplayScoreboard(board, DisplayScoreboardPacket.ScoreboardPosition.Sidebar);
                                        break;
                                    case "update":
                                        minecraftServer.ScoreboardManager[parameters[1]][parameters[2]]++;
                                        break;
                                    case "remove":
                                        minecraftServer.ScoreboardManager.RemoveScoreboard(parameters[1]);
                                        break;
                                }
                            }
                            catch
                            {
                                e.Origin.SendChat("Usage: /scoreboard add/remove/update name title/score");
                            }
                            break;

                        case "team":
                            try
                            {
                                switch (parameters[0])
                                {
                                    case "createteam":
                                        minecraftServer.ScoreboardManager.CreateTeam(parameters[1], parameters[2],
                                            true, ChatColors.Delimiter + parameters[2], ChatColors.Plain);
                                        break;
                                    case "setteam":
                                        var team = minecraftServer.ScoreboardManager.GetTeam(parameters[1]);
                                        team.AddPlayers(parameters.Skip(1).ToArray());
                                        break;
                                }
                            }
                            catch
                            {
                                e.Origin.SendChat("Usage: /team add/create name displayname");
                            }
                            break;
                        case "gamemode":
                            if (parameters.Length == 1)
                            {
                                if (parameters[0] == "c" || parameters[0] == "creative" || parameters[0] == "1")
                                {
                                    e.Origin.Entity.GameMode = GameMode.Creative;
                                }
                                if (parameters[0] == "s" || parameters[0] == "survival" || parameters[0] == "0")
                                {
                                    e.Origin.Entity.GameMode = GameMode.Survival;
                                }
                                if (parameters[0] == "a" || parameters[0] == "adventure" || parameters[0] == "2")
                                {
                                    e.Origin.Entity.GameMode = GameMode.Creative;
                                }
                            }
                            else
                            {
                                e.Origin.SendChat("Usage: /gamemode c/s/a");
                            }
                            break;
                        case "tp":
                            if (parameters.Length == 3)
                            {
                                minecraftServer.EntityManager.TeleportEntity(e.Origin.Entity, new Vector3(double.Parse(parameters[0]), double.Parse(parameters[1]), double.Parse(parameters[2])));
                            }
                            else
                            {
                                e.Origin.SendChat("Usage: /tp [x] [y] [z]");
                            }
                            break;
                    }

                }
                else
                {
                    e.Origin.SendChat(ChatColors.Red + "Permission Denied");
                }
        }
    }
  }
}
