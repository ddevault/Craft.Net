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
            MinecraftServer minecraftServer = new MinecraftServer(
		        new IPEndPoint(IPAddress.Any, 25565));
            minecraftServer.AddLogProvider(new ConsoleLogWriter());
            minecraftServer.AddWorld(new World());
            minecraftServer.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
            minecraftServer.Stop();
        }
    }
}
