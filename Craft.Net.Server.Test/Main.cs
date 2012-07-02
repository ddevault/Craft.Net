using System;
using Craft.Net.Server;
using System.Net;
using Craft.Net.Server.Worlds;

namespace Craft.Net.Server.Test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Craft.Net Test Server");
            MinecraftServer minecraftServer = new MinecraftServer(
		        new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.AddWorld(new World());
            minecraftServer.Start();
            Console.WriteLine("Server started. Press any key to exit.");
            Console.ReadKey(true);
            minecraftServer.Stop();
        }
    }
}
