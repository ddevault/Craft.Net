using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.Server;
using Craft.Net.Server.Events;
using Craft.Net.TerrainGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Craft.Net.Networking;

namespace TestServer
{
    class Program
    {
        static Level level;
        static MinecraftServer server;

        static void Main(string[] args)
        {
            if (Directory.Exists("world"))
                Directory.Delete("world", true);
            level = new Level(new StandardGenerator(), "world");
            level.AddWorld("region");
            level.AddWorld("test", new FlatlandGenerator());
            level.Worlds[1].GenerateChunk(Coordinates2D.Zero);
            level.SaveTo("world");
            server = new MinecraftServer(level);
            server.ChatMessage += server_ChatMessage;
            server.Settings.OnlineMode = true;
            server.Settings.MotD = "Craft.Net Test Server";
            server.Start(new IPEndPoint(IPAddress.Any, 25565));
            Console.WriteLine("Press 'q' to exit");
            ConsoleKeyInfo cki;
            do cki = Console.ReadKey(true);
            while (cki.KeyChar != 'q');
            server.Stop();
        }

        static void server_ChatMessage(object sender, ChatMessageEventArgs e)
        {
            if (e.Message.RawMessage.StartsWith("/"))
            {
                string command = e.Message.FullText();
                e.Handled = true;
                if (command == "/creative")
                    e.Origin.GameMode = GameMode.Creative;
                else if (command == "/survival")
                    e.Origin.GameMode = GameMode.Survival;
                else if (command == "/world2")
                    server.MoveClientToWorld(e.Origin, server.GetWorld("test"));
                else if (command == "/world1")
                    server.MoveClientToWorld(e.Origin, server.GetWorld("region"));
            }
        }
    }
}
