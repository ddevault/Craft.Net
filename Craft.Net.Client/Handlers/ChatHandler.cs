﻿namespace Craft.Net.Client.Handlers
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
            if (String.IsNullOrEmpty(message))
                return string.Empty;
 
            int idx = 0;
            var chars = new char[message.Length];

            for (int i = 0; i < message.Length; ++i) {
                if (message[i] != ChatColors.Delimiter[0])
                    chars[idx++] = message[i];
                else
                    i++;
            }

            if (idx > 0)
                return new string(chars, 0, idx);

            return string.Empty; 
        }
    }
}
