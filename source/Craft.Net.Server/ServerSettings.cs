using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Craft.Net.Common;

namespace Craft.Net.Server
{
    public class ServerSettings
    {
        public static ServerSettings DefaultSettings
        {
            get
            {
                var settings = new ServerSettings();
                settings.DefaultWorldIndex = 0;
                settings.EnableEncryption = true;
                settings.MaxPlayers = 25;
                settings.MotD = "Craft.Net Server";
                settings.OnlineMode = true;
                settings.Difficulty = Difficulty.Normal;
                settings.SaveInterval = 30;
                return settings;
            }
        }

        /// <summary>
        /// The default world to spawn new players in.
        /// </summary>
        public int DefaultWorldIndex { get; set; }
        /// <summary>
        /// Determines whether or not encryption should
        /// be in use on this server.
        /// </summary>
        public bool EnableEncryption { get; set; }
        /// <summary>
        /// The maximum number of players that may log in.
        /// </summary>
        public byte MaxPlayers { get; set; }
        /// <summary>
        /// The message of the day.
        /// </summary>
        public string MotD { get; set; }
        /// <summary>
        /// Set to true to authenticate connecting users with Minecraft.net
        /// </summary>
        public bool OnlineMode { get; set; }
        /// <summary>
        /// This server's difficulty.
        /// </summary>
        public Difficulty Difficulty;
        /// <summary>
        /// The number of seconds between saving the level to disk. Set to -1 to disable periodic
        /// saves.
        /// </summary>
        public int SaveInterval { get; set; }
    }
}
