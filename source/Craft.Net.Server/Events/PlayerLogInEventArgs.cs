using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Server.Events
{
    /// <summary>
    /// Used to describe a new player joining.
    /// </summary>
    public class PlayerLogInEventArgs : EventArgs
    {
        public string Username
        {
            get { return Client.Username; }
        }

        public RemoteClient Client { get; set; }

        public bool Handled { get; set; }

        public PlayerLogInEventArgs(RemoteClient client)
        {
            Client = client;
        }
    }
}
