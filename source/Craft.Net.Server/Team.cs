using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Craft.Net.Server
{
    public class Team
    {
        internal Team(MinecraftServer server, ScoreboardManager scoreboardManager, string name, 
            string displayName, bool allowFriendlyFire, string playerPrefix, string playerSuffix)
        {
            Server = server;
            ScoreboardManager = scoreboardManager;
            Name = name;
            _allowFriendlyFire = allowFriendlyFire;
            _displayName = displayName;
            _playerPrefix = playerPrefix;
            _playerSuffix = playerSuffix;
            Players = new List<string>();
        }

        private MinecraftServer Server { get; set; }
        private ScoreboardManager ScoreboardManager { get; set; }

        public string Name { get; internal set; }
        private bool _allowFriendlyFire;
        public bool AllowFriendlyFire
        {
            get { return _allowFriendlyFire; }
            set
            {
                _allowFriendlyFire = value;
                foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                {
                    client.SendPacket(SetTeamsPacket.UpdateTeam(Name, DisplayName, PlayerPrefix, 
                        PlayerSuffix, AllowFriendlyFire));
                }
            }
        }
        public bool ShowInvisibleAllies { get; set; }
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                {
                    client.SendPacket(SetTeamsPacket.UpdateTeam(Name, DisplayName, PlayerPrefix,
                        PlayerSuffix, AllowFriendlyFire));
                }
            }
        }
        private string _playerPrefix;
        public string PlayerPrefix
        {
            get { return _playerPrefix;  }
            set
            {
                _playerPrefix = value;
                foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                {
                    client.SendPacket(SetTeamsPacket.UpdateTeam(Name, DisplayName, PlayerPrefix,
                        PlayerSuffix, AllowFriendlyFire));
                }
            }
        }
        private string _playerSuffix;
        public string PlayerSuffix 
        {
            get { return _playerSuffix;  }
            set
            {
                _playerSuffix = value;
                foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                {
                    client.SendPacket(SetTeamsPacket.UpdateTeam(Name, DisplayName, PlayerPrefix,
                        PlayerSuffix, AllowFriendlyFire));
                }
            }
        }

        public void AddPlayers(params string[] players)
        {
            if (Players.Any(players.Contains))
                throw new DuplicateKeyException("The players being added are already on this team.");
            Players.AddRange(players);
            foreach (var team in ScoreboardManager.Teams.Where(t => t != this))
            {
                foreach (var player in players.Where(team.Players.Contains))
                    team.RemovePlayers(player);
            }
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(SetTeamsPacket.AddPlayers(Name, players));
        }

        public void RemovePlayers(params string[] players)
        {
            if (players.Any(p => !Players.Contains(p)))
                throw new KeyNotFoundException("The specified players are not on this team.");
            foreach (var p in players)
                Players.Remove(p);
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(SetTeamsPacket.RemovePlayers(Name, players));
        }

        public string[] GetPlayers()
        {
            return Players.ToArray();
        }

        internal List<string> Players { get; set; }
    }
}
