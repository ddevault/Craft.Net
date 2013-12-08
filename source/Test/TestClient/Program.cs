using System;
using System.Net;
using System.Net.Sockets;
using Craft.Net.Client;
using System.Linq;

namespace TestClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            var endPoint = ParseEndPoint(args[0]);
            var ping = ServerPing.DoPing(endPoint);
            Console.WriteLine("{0}/{1} {2} ({3}): {4} [{5}ms latency]",
                ping.Players.OnlinePlayers,
                ping.Players.MaxPlayers,
                ping.Version.Name,
                ping.Version.ProtocolVersion,
                ping.Description,
                (int)ping.Latency.TotalMilliseconds);
            Console.WriteLine("Player list sample:");
            if (ping.Players.Players != null)
                foreach (var player in ping.Players.Players)
                    Console.WriteLine("{0} ({1})", player.Name, player.Id);
            if (!string.IsNullOrEmpty(ping.Icon))
                Console.WriteLine("Server icon: {0}", ping.Icon);

            //var lastLogin = LastLogin.GetLastLogin();
            //var session = Session.DoLogin(lastLogin.Username, lastLogin.Password);
            var session = new Session("TestBot");

            // Connect to server
            var client = new MinecraftClient(session);
            client.Connect(endPoint);

            client.ChatMessage += (sender, e) => Console.WriteLine(e.RawMessage);
            string command;
            do
            {
                command = Console.ReadLine();
                if (command == null)
                    continue; // MonoDevelop debugger does this sometimes
                if (command.StartsWith("say "))
                    client.SendChat(command.Substring(4));
            } while (command != "quit");

            client.Disconnect("Quitting");
        }

        private static IPEndPoint ParseEndPoint(string arg)
        {
            IPAddress address;
            int port;
            if (arg.Contains(":"))
            {
                // Both IP and port are specified
                var parts = arg.Split(':');
                if (!IPAddress.TryParse(parts[0], out address))
                    address = Resolve(parts[0]);
                return new IPEndPoint(address, int.Parse(parts[1]));
            }
            if (IPAddress.TryParse(arg, out address))
                return new IPEndPoint(address, 25565);
            if (int.TryParse(arg, out port))
                return new IPEndPoint(IPAddress.Loopback, port);
            return new IPEndPoint(Resolve(arg), 25565);
        }

        private static IPAddress Resolve(string arg)
        {
            return Dns.GetHostEntry(arg).AddressList.FirstOrDefault(item => item.AddressFamily == AddressFamily.InterNetwork);
        }
    }
}
