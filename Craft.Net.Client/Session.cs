using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Client
{
    public class Session
    {
        private const string LoginUrl = "https://login.minecraft.net?user={0}&password={1}&version=13";

        public static Session DoLogin(string username, string password)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(LoginUrl,
                Uri.EscapeUriString(username),
                Uri.EscapeUriString(password)));
            var response = request.GetResponse();
            StreamReader responseStream = new StreamReader(response.GetResponseStream());
            string login = responseStream.ReadToEnd();
            responseStream.Close();
            if (login.Count(c => c == ':') != 4)
                throw new UnauthorizedAccessException(login);
            string[] parts = login.Split(':');
            return new Session(parts[2], parts[3], parts[0]);
        }

        public Session(string username)
        {
            Username = username;
        }

        public Session(string username, string sessionId) : this(username)
        {
            SessionId = sessionId;
        }

        public Session(string username, string sessionId, string version)
            : this(username, sessionId)
        {
            Version = version;
        }

        public string Username { get; set; }
        public string SessionId { get; set; }
        public string Version { get; set; }
    }
}
