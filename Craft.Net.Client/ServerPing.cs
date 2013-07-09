using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Client
{
    public class ServerPing
    {
        public static ServerPing DoPing(string address)
        {
            return DoPing(MinecraftClient.ParseEndPoint(address));
        }

        public static ServerPing DoPing(IPEndPoint endPoint)
        {
            var client = new TcpClient();
            client.Connect(endPoint);
            var stream = new MinecraftStream(client.GetStream());
            var ping = new ServerListPingPacket();
            ping.WritePacket(stream);
            var response = PacketReader.ReadPacket(stream);
            client.Close();
            // TODO: Handle old pings
            var pong = (DisconnectPacket)response;
            var parts = pong.Reason.Substring(3).Split('\0');
            var result = new ServerPing();
            result.ProtocolVersion = int.Parse(parts[0]);
            result.ServerVersion = parts[1];
            result.MotD = parts[2];
            result.CurrentPlayers = int.Parse(parts[3]);
            result.MaxPlayers = int.Parse(parts[4]);
            return result;
        }

        public int ProtocolVersion { get; set; }
        public string ServerVersion { get; set; }
        public string MotD { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }
}
