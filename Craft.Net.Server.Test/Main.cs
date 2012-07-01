using System;
using Craft.Net.Server;
using System.Net;

namespace Craft.Net.Server.Test
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting Craft.Net Test Server");
            MinecraftServer minecraftServer = new MinecraftServer(
		        new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.Start();
            Console.WriteLine("Server started. Press any key to exit.");
            Console.ReadKey(true);
            minecraftServer.Stop();
        }
    }
}
