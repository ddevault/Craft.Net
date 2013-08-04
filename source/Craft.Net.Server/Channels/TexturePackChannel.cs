using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Craft.Net.Server.Events;

namespace Craft.Net.Server.Channels
{
    /// <summary>
    /// Implements the vanilla custom texture pack plugin channel.
    /// </summary>
    public class TexturePackChannel : PluginChannel
    {
        public string DownloadUrl { get; set; }
        public int Resolution { get; set; }

        public TexturePackChannel(string downloadUrl)
        {
            DownloadUrl = downloadUrl;
            Resolution = 16;
        }

        public TexturePackChannel(string downloadUrl, int resolution)
        {
            DownloadUrl = downloadUrl;
            Resolution = resolution;
        }

        public override string Channel
        {
            get { return "MC|TPack"; }
        }

        public override void MessageRecieved(RemoteClient client, byte[] data)
        {
            throw new InvalidOperationException();
        }

        public override void ChannelRegistered(MinecraftServer server)
        {
            server.PlayerLoggedIn += ServerOnPlayerLoggedIn;
        }

        private void ServerOnPlayerLoggedIn(object sender, PlayerLogInEventArgs eventArgs)
        {
            byte[] payload = Encoding.ASCII.GetBytes(DownloadUrl)
                .Concat(new byte[] { 0x00 })
                .Concat(Encoding.ASCII.GetBytes("16")).ToArray();
            SendMessage(payload, eventArgs.Client);
        }
    }
}