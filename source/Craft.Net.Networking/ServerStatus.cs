using System;
using Newtonsoft.Json;

namespace Craft.Net
{
    public class ServerStatus
    {
        public class ServerVersion
        {
            public ServerVersion()
            {
            }

            public ServerVersion(string name, long protocolVersion)
            {
                Name = name;
                ProtocolVersion = protocolVersion;
            }

            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("protocol")]
            public long ProtocolVersion { get; set; }
        }

        public class PlayerList
        {
            public class Player
            {
                public Player()
                {
                }

                public Player(string name, string id)
                {
                    Name = name;
                    Id = id;
                }

                [JsonProperty("name")]
                public string Name { get; set; }
                [JsonProperty("id")]
                public string Id { get; set; }
            }

            public PlayerList()
            {
            }

            public PlayerList(int maxPlayers, int onlinePlayers, Player[] players)
            {
                MaxPlayers = maxPlayers;
                OnlinePlayers = onlinePlayers;
                Players = players;
            }

            [JsonProperty("max")]
            public int MaxPlayers { get; set; }
            [JsonProperty("online")]
            public int OnlinePlayers { get; set; }
            [JsonProperty("sample")]
            public Player[] Players { get; set; }
        }

        public ServerStatus()
        {
        }

        public ServerStatus(ServerVersion version, PlayerList players, string description, string icon)
        {
            Version = version;
            Players = players;
            Description = description;
            Icon = icon;
        }

        [JsonProperty("version")]
        public ServerVersion Version { get; set; }
        [JsonProperty("players")]
        public PlayerList Players { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("favicon")]
        public string Icon { get; set; }
        [JsonIgnore]
        public TimeSpan Latency { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}