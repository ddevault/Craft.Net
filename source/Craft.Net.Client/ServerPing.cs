using System;
using System.Net;
using System.Net.Sockets;
using Craft.Net.Networking;
using Craft.Net.Common;

namespace Craft.Net.Client
{
    public static class ServerPing
    {
        public static ServerStatus DoPing(IPEndPoint endPoint, string hostname = null)
        {
            var client = new TcpClient();
            client.Connect(endPoint);
            var manager = new NetworkManager(client.GetStream());
            manager.WritePacket(new HandshakePacket(
                NetworkManager.ProtocolVersion,
                hostname ?? endPoint.Address.ToString(),
                (ushort)endPoint.Port,
                NetworkMode.Status), PacketDirection.Serverbound);
            manager.WritePacket(new StatusRequestPacket(), PacketDirection.Serverbound);
            var _response = manager.ReadPacket(PacketDirection.Clientbound);
            if (!(_response is StatusResponsePacket))
            {
                client.Close();
                throw new InvalidOperationException("Server returned invalid ping response");
            }
            var response = (StatusResponsePacket)_response;
            var sent = DateTime.Now;
            manager.WritePacket(new StatusPingPacket(sent.Ticks), PacketDirection.Serverbound);
            var _pong = manager.ReadPacket(PacketDirection.Clientbound);
            if (!(_pong is StatusPingPacket))
            {
                client.Close();
                throw new InvalidOperationException("Server returned invalid ping response");
            }
            client.Close();
            var pong = (StatusPingPacket)_pong;
            var time = new DateTime(pong.Time);
            response.Status.Latency = time - sent;
            return response.Status;
        }
    }
}