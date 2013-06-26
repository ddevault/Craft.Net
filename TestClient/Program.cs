using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            var ping = ServerPing.DoPing(args[0]);
            Console.WriteLine("{0}/{1} {2} ({3}): {4}", ping.CurrentPlayers, ping.MaxPlayers, ping.ServerVersion,
                ping.ProtocolVersion, ping.MotD);

            //var lastLogin = LastLogin.GetLastLogin();
            //var session = Session.DoLogin(lastLogin.Username, lastLogin.Password);
            var session = new Session("TestBot");

            // Connect to server
            var client = new MinecraftClient(session);
            client.Connect(args[0]);

            client.PlayerDied += (s, e) => Console.WriteLine("Player died! Type 'respawn' to respawn.");
            client.Disconnected += (s, e) => Console.WriteLine("Disconnected: " + e.Reason);

            string input = "";
            while (input != "quit")
            {
                input = Console.ReadLine();
                if (input == null) continue;

                if (input.StartsWith("move "))
                {
                    var parts = input.Split(' ');
                    var amountX = int.Parse(parts[1]);
                    var amountZ = int.Parse(parts[2]);

                    client.Move(amountX, amountZ);
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
                else if (input.StartsWith("lookat "))
                {
                    var parts = input.Split(' ');
                    var pos = new Vector3
                    {
                        X = double.Parse(parts[1]),
                        Y = double.Parse(parts[2]),
                        Z = double.Parse(parts[3])
                    };

                    client.LookAt(pos);
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
    }
}
