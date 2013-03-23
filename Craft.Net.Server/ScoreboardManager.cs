using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Craft.Net.Server.Events;

namespace Craft.Net.Server
{
    public class ScoreboardManager
    {
        private MinecraftServer Server { get; set; }
        private List<Scoreboard> Scoreboards { get; set; }
        internal List<Team> Teams { get; set; }

        internal ScoreboardManager(MinecraftServer server)
        {
            Server = server;
            Scoreboards = new List<Scoreboard>();
            Teams = new List<Team>();
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
                throw new KeyNotFoundException("This scoreboard is not known to the server.");
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new CreateScoreboardPacket(scoreboard.Name, scoreboard.DisplayName, true));
            Scoreboards.Remove(scoreboard);
        }

        public Scoreboard[] GetScoreboards()
        {
            return Scoreboards.ToArray();
        }

        public Scoreboard GetScoreboard(string name)
        {
            return Scoreboards.First(s => s.Name == name);
        }

        public Team GetTeam(string name)
        {
            return Teams.First(t => t.Name == name);
        }

        public Team CreateTeam(string name, string displayName, bool allowFriendlyFire, 
            string playerPrefix, string playerSuffix)
        {
            if (Teams.Any(t => t.Name == name))
                throw new DuplicateKeyException("The specified team already exists.");
            var team = new Team(Server, this, name, displayName, allowFriendlyFire, playerPrefix, playerSuffix);
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(SetTeamsPacket.CreateTeam(name, displayName, playerPrefix, playerSuffix, allowFriendlyFire, new string[0]));
            Teams.Add(team);
            return team;
        }

        public void RemoveTeam(string name)
        {
            RemoveTeam(GetTeam(name));
        }

        public void RemoveTeam(Team team)
        {
            if (!Teams.Contains(team))
                throw new KeyNotFoundException("This team is not known to the server.");
            Teams.Remove(team);
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(SetTeamsPacket.RemoveTeam(team.Name));
        }

        public Team GetPlayerTeam(string name)
        {
            return Teams.FirstOrDefault(t => t.Players.Contains(name));
        }

        public Scoreboard this[string name]
        {
            get { return GetScoreboard(name); }
        }
    }
}
