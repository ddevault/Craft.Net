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
using Craft.Net.Logic.Blocks;

namespace TestServer
{
    class Program
    {
        static Level level;

        static void Main(string[] args)
        {
            if (Directory.Exists("world"))
                Directory.Delete("world", true);
            level = new Level(new StandardGenerator(), "world");
            level.AddWorld("region");
            level.SaveTo("world");
            var server = new MinecraftServer(level);
            server.ChatMessage += server_ChatMessage;
            server.PlayerLoggedIn += server_PlayerLoggedIn;
            server.Settings.OnlineMode = false;
            server.Start(new IPEndPoint(IPAddress.Any, 25565));
            Console.WriteLine("Press 'q' to exit");
            ConsoleKeyInfo cki;
            do cki = Console.ReadKey(true);
            while (cki.KeyChar != 'q');
            server.Stop();
        }

        static void server_PlayerLoggedIn(object sender, PlayerLogInEventArgs e)
        {
            if (e.Client.Entity.Inventory.Empty)
            {
                e.Client.Entity.Inventory.Hotbar[0] = new ItemStack(CobblestoneBlock.BlockId, 64);
                e.Client.Entity.Inventory.Hotbar[1] = new ItemStack(CobblestoneBlock.BlockId, 64);
                e.Client.Entity.Inventory.Hotbar[2] = new ItemStack(DirtBlock.BlockId, 32);
            }
        }

        static void server_ChatMessage(object sender, ChatMessageEventArgs e)
        {
            if (e.RawMessage.StartsWith("/"))
            {
                e.Handled = true;
                if (e.RawMessage == "/creative")
                    e.Origin.GameMode = GameMode.Creative;
                else if (e.RawMessage == "/survival")
                    e.Origin.GameMode = GameMode.Survival;
            }
        }
    }
}
