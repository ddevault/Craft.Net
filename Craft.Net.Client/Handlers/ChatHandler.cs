namespace Craft.Net.Client.Handlers
{
    using System.Text.RegularExpressions;
    using Events;

    public class ChatHandler
    {
        private static readonly Regex MessageFilter = new Regex("(§.)*");

        public static void ChatMessage(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            LogProvider.Log(packet.Message, LogImportance.High);


            client.OnChatMessage(new ChatMessageEventArgs(packet.Message, MessageFilter.Replace(packet.Message, string.Empty)));
        }
    }
}