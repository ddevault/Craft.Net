using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using Craft.Net.Server.Events;

namespace Craft.Net.Server
{
    public class ScoreboardManager
    {
        private MinecraftServer Server { get; set; }
        private List<Scoreboard> Scoreboards { get; set; }

        internal ScoreboardManager(MinecraftServer server)
        {
            Server = server;
            Scoreboards = new List<Scoreboard>();
            Server.PlayerLoggedIn += ServerOnPlayerLoggedIn;
        }

        private void ServerOnPlayerLoggedIn(object sender, PlayerLogInEventArgs playerLogInEventArgs)
        {
            var client = playerLogInEventArgs.Client;
            foreach (var board in Scoreboards)
            {
                client.SendPacket(new CreateScoreboardPacket(board.Name, board.DisplayName));
                foreach (var score in board.Scores)
                    client.SendPacket(new UpdateScorePacket(score.Key, board.Name, score.Value));
            }
        }

        public Scoreboard CreateScoreboard(string name, string displayName)
        {
            var board = new Scoreboard(Server, name, displayName);
            Scoreboards.Add(board);
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new CreateScoreboardPacket(name, displayName));
            return board;
        }

        public void DisplayScoreboard(Scoreboard scoreboard, DisplayScoreboardPacket.ScoreboardPosition position)
        {
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new DisplayScoreboardPacket(position, scoreboard.Name));
        }

        public void RemoveScoreboard(string scoreboard)
        {
            RemoveScoreboard(this[scoreboard]);
        }

        public void RemoveScoreboard(Scoreboard scoreboard)
        {
            if (!Scoreboards.Contains(scoreboard))
                throw new InstanceNotFoundException("This scoreboard is not known to the server.");
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new CreateScoreboardPacket(scoreboard.Name, scoreboard.DisplayName, true));
            Scoreboards.Remove(scoreboard);
        }

        public Scoreboard[] GetScoreboards()
        {
            return Scoreboards.ToArray();
        }

        public Scoreboard this[string key]
        {
            get { return Scoreboards.First(s => s.Name == key); }
        }
    }
}
