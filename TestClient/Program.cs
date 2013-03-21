using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Client;
using Craft.Net.Data;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up log providers
            LogProvider.RegisterProvider(new FileLogWriter("packetLog.txt", LogImportance.Low));
            LogProvider.RegisterProvider(new ConsoleLogWriter(LogImportance.High));

            // Set up endpoint, ping server
            var endPoint = ParseEndPoint(args[0]);
            var ping = ServerPing.DoPing(endPoint);
            Console.WriteLine("{0}/{1} {2} ({3}): {4}", ping.CurrentPlayers, ping.MaxPlayers, ping.ServerVersion,
                ping.ProtocolVersion, ping.MotD);

            //var lastLogin = LastLogin.GetLastLogin();
            //var session = Session.DoLogin(lastLogin.Username, lastLogin.Password);
            var session = new Session("TestBot");

            // Connect to server
            var client = new MinecraftClient(session);
            client.Connect(endPoint);

            client.PlayerDied += (s, e) => Console.WriteLine("Player died! Type 'respawn' to respawn.");
            client.Disconnected += (s, e) => Console.WriteLine("Disconnected: " + e.Reason);

            string input = "";
            while (input != "quit")
            {
                input = Console.ReadLine();
                if (input.StartsWith("move "))
                {
                    var parts = input.Split(' ');
                    var amount = double.Parse(parts[2]);
                    var position = client.Position;
                    if (parts[1] == "x")
                        position.X += amount;
                    else if (parts[1] == "y")
                        position.Y += amount;
                    else
                        position.Z += amount;
                    client.Position = position;
                }
                else if (input.StartsWith("look "))
                {
                    var parts = input.Split(' ');
                    var amount = float.Parse(parts[2]);
                    if (parts[1] == "yaw")
                        client.Yaw = amount;
                    else
                        client.Pitch = amount;
                }
                else if (input.StartsWith("say "))
                    client.SendChat(input.Substring(4));
                else if (input == "respawn")
                    client.Respawn();
                else if (input == "save")
                    client.World.Save("testWorld", true);
                else if (input == "under")
                    Console.WriteLine(client.World.GetBlock(client.Position + Vector3.Down).GetType().Name);
            }

            client.Disconnect("Quitting");
        }

        private static IPEndPoint ParseEndPoint(string arg)
        {
            IPAddress address;
            int port;
            if (arg.Contains(':'))
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
            return Dns.GetHostEntry(arg).AddressList.FirstOrDefault();
        }
    }
}
