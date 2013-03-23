using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client
{
    public class LevelInformation
    {
        internal LevelInformation(LoginRequestPacket packet)
        {
            LevelType = packet.LevelType;
            Dimension = packet.Dimension;
            Difficulty = packet.Difficulty;
            GameMode = packet.GameMode;
        }

        public string LevelType { get; private set; }
        public Dimension Dimension { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public GameMode GameMode { get; private set; }
    }
}
