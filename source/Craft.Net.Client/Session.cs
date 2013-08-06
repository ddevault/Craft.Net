using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Craft.Net.Client
{
    public class Session
    {
        private const string LoginUrl = "https://login.minecraft.net?user={0}&password={1}&version=13";

        public static Session DoLogin(string username, string password)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format(LoginUrl,
                                                                          Uri.EscapeUriString(username),
                                                                          Uri.EscapeUriString(password)));
            var response = request.GetResponse();
            var responseStream = new StreamReader(response.GetResponseStream());
            string login = responseStream.ReadToEnd();
            responseStream.Close();
            if (login.Count(c => c == ':') != 4)
                throw new UnauthorizedAccessException(login);
            string[] parts = login.Split(':');
            return new Session(parts[2], parts[3], parts[0], parts[4]);
        }

        public Session(string username, string sessionId = null, string version = null, string userId = null)
        {
            Username = username;
            SessionId = sessionId;
            Version = version;
            UserId = userId;
        }

        public string Username { get; set; }
        public string SessionId { get; set; }
        public string Version { get; set; }
        public string UserId { get; set; }
    }
}