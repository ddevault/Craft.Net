namespace Craft.Net.Client.Handlers
{
    using Craft.Net.Data;

    using Events;

    public class ChatHandler
    {
        public static void ChatMessage(MinecraftClient client, IPacket _packet)
        {
            var packet = (ChatMessagePacket)_packet;
            LogProvider.Log(packet.Message, LogImportance.High);

            client.OnChatMessage(new ChatMessageEventArgs(packet.Message, RemoveChatCodes(packet.Message)));
        }

        private static string RemoveChatCodes(string message)
        {
            int index;
            while ((index = message.IndexOf(ChatColors.Delimiter)) >= 0)
                message = message.Remove(index, 2);

            return message;
        }
    }
}