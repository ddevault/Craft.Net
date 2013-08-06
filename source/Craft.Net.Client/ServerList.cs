using System.Collections.Generic;
using System.IO;
using fNbt;

namespace Craft.Net.Client
{
    /// <summary>
    /// Provides functionality for interacting with
    /// the saved vanilla server list.
    /// </summary>
    public class ServerList
    {
        public static string ServersDat
        {
            get
            {
                return Path.Combine(DotMinecraft.GetDotMinecraftPath(), "servers.dat");
            }
        }

        public ServerList()
        {
            Servers = new List<Server>();
        }

        public List<Server> Servers { get; set; }

        public void Save()
        {
            SaveTo(ServersDat);
        }

        public void SaveTo(string file)
        {
            var nbt = new NbtFile(file);
            nbt.RootTag = new NbtCompound("");
            var list = new NbtList("servers", NbtTagType.Compound);
            foreach (var server in Servers)
            {
                var compound = new NbtCompound();
                compound.Add(new NbtString("name", server.Name));
                compound.Add(new NbtString("ip", server.Ip));
                compound.Add(new NbtByte("hideAddress", (byte)(server.HideAddress ? 1 : 0)));
                compound.Add(new NbtByte("acceptTextures", (byte)(server.AcceptTextures ? 1 : 0)));
                list.Add(compound);
            }
            nbt.RootTag.Add(list);
            nbt.SaveToFile(file, NbtCompression.None);
        }

        public static ServerList Load()
        {
            return LoadFrom(ServersDat);
        }

        public static ServerList LoadFrom(string file)
        {
            var list = new ServerList();
            var nbt = new NbtFile(file);
            foreach (NbtCompound server in nbt.RootTag["servers"] as NbtList)
            {
                var entry = new Server();
                if (server.Contains("name"))
                    entry.Name = server["name"].StringValue;
                if (server.Contains("ip"))
                    entry.Ip = server["ip"].StringValue;
                if (server.Contains("hideAddress"))
                    entry.HideAddress = server["hideAddress"].ByteValue == 1;
                if (server.Contains("acceptTextures"))
                    entry.AcceptTextures = server["acceptTextures"].ByteValue == 1;
                list.Servers.Add(entry);
            }
            return list;
        }

        public class Server
        {
            public string Name { get; set; }
            public string Ip { get; set; }
            public bool HideAddress { get; set; }
            public bool AcceptTextures { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}