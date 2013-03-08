using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;

namespace Craft.Net.Server
{
    public class Scoreboard
    {
        internal Dictionary<string, int> Scores { get; set; }

        internal Scoreboard(MinecraftServer server, string name, string displayName)
        {
            Server = server;
            Name = name;
            DisplayName = displayName;
            Scores = new Dictionary<string, int>();
        }

        private MinecraftServer Server { get; set; }

        public string Name { get; internal set; }
        public string DisplayName { get; internal set; }

        public void AddScore(string name, int value)
        {
            if (Scores.ContainsKey(name))
                throw new DuplicateKeyException(name);
            Scores[name] = value;
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new UpdateScorePacket(name, Name, value));
        }

        public void RemoveScore(string name)
        {
            if (!Scores.ContainsKey(name))
                throw new InstanceNotFoundException("The specified score does not exist.");
            foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                client.SendPacket(new UpdateScorePacket(name));
            Scores.Remove(name);
        }

        public string[] GetScoreNames()
        {
            return Scores.Keys.ToArray();
        }

        public int this[string key]
        {
            get { return Scores[key]; }
            set
            {
                Scores[key] = value;
                foreach (var client in Server.Clients.Where(c => c.IsLoggedIn))
                    client.SendPacket(new UpdateScorePacket(key, Name, value));
            }
        }
    }
}
