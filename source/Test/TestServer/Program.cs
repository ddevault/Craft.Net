using Craft.Net.Anvil;
using Craft.Net.Server;
using Craft.Net.TerrainGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var level = new Level(new FlatlandGenerator(), "world");
            level.AddWorld("region");
            level.SaveTo("world");
            var server = new MinecraftServer(level);
            server.Settings.OnlineMode = false;
            server.Start(new IPEndPoint(IPAddress.Any, 25565));
            Console.WriteLine("Press 'q' to exit");
            ConsoleKeyInfo cki;
            do cki = Console.ReadKey(true);
            while (cki.KeyChar != 'q');
            server.Stop();
        }
    }
}
