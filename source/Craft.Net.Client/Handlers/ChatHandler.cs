using Craft.Net.Networking;
using Craft.Net.Client.Events;

namespace Craft.Net.Client.Handlers
{
    public class ChatHandler
    {
        public static void ChatMessage(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            client.OnChatMessage(new ChatMessageEventArgs(packet.Message));
        }
    }
}