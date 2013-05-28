using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Client.Events
{
    public class GameModeChangeEventArgs : EventArgs
    {
        public GameMode OldGameMode { get; set; }
        public GameMode GameMode { get; set; }

        public GameModeChangeEventArgs(GameMode oldGameMode, GameMode gameMode)
        {
            OldGameMode = oldGameMode;
            GameMode = gameMode;
        }
    }
}
