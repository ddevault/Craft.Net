using System;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Craft.Net.Client
{
    public class Session
    {
        public static Session DoLogin(string username, string password)
        {
            var serializer = new JsonSerializer();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/authenticate");
                request.ContentType = "application/json";
                request.Method = "POST";
                var token = Guid.NewGuid().ToString();
                var blob = new AuthenticationBlob(username, password, token);
                var json = JsonConvert.SerializeObject(blob);
                var stream = request.GetRequestStream();
                using (var writer = new StreamWriter(stream))
                    writer.Write(json);
                var response = request.GetResponse();
                stream = response.GetResponseStream();
                var session = serializer.Deserialize<Session>(new JsonTextReader(new StreamReader(stream)));
                session.UserName = username;
                return session;
            }
            catch (WebException e)
            {
                var stream = e.Response.GetResponseStream();
                var json = new StreamReader(stream).ReadToEnd();
                stream.Close();
                throw JsonConvert.DeserializeObject<MinecraftAuthenticationException>(json);
            }
        }

        private class AuthenticationBlob
        {
            public AuthenticationBlob(string username, string password, string token)
            {
                Username = username;
                Password = password;
                ClientToken = token;
                Agent = new AgentBlob();
                RequestUser = true;
            }

            public class AgentBlob
            {
                public AgentBlob()
                {
                    // TODO: Update if needed, per https://twitter.com/sircmpwn/status/365306166638690304
                    Name = "Minecraft";
                    Version = 1;
                }

                [JsonProperty("name")]
                public string Name { get; set; }
                [JsonProperty("version")]
                public int Version { get; set; }
            }

            [JsonProperty("agent")]
            public AgentBlob Agent { get; set; }
            [JsonProperty("username")]
            public string Username { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }
            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
            [JsonProperty("requestUser")]
            public bool RequestUser { get; set; }
        }

        private class RefreshBlob
        {
            public RefreshBlob(Session session)
            {
                AccessToken = session.AccessToken;
                ClientToken = session.ClientToken;
                SelectedProfile = session.SelectedProfile;
                RequestUser = true;
            }

            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }
            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
            [JsonProperty("selectedProfile")]
            public Profile SelectedProfile { get; set; }
            [JsonProperty("requestUser")]
            public bool RequestUser { get; set; }
            [JsonProperty("user")]
            public MojangUser User { get; set; }
        }

        public class MinecraftAuthenticationException : Exception
        {
            public MinecraftAuthenticationException()
            {
            }

            [JsonProperty("error")]
            public string Error { get; set; }
            [JsonProperty("errorMessage")]
            public string ErrorMessage { get; set; }
            [JsonProperty("cause")]
            public string Cause { get; set; }
        }

        public class Profile
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }
                public class MojangUser
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("properties")]
            public IList<Userproperty> Properties { get; set; }
        }

        public class Userproperty
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public Session(string userName)
        {
            AvailableProfiles = new Profile[]
            {
                new Profile
                {
                    Name = userName,
                    Id = Guid.NewGuid().ToString()
                }
            };
            SelectedProfile = AvailableProfiles[0];
        }

        /// <summary>
        /// Refreshes this session, so it may be used again. You will need to re-save it if you've
        /// saved it to disk.
        /// </summary>
        public void Refresh()
        {
            if (!OnlineMode)
                throw new InvalidOperationException("This is an offline-mode session.");
            var serializer = new JsonSerializer();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://authserver.mojang.com/refresh");
                request.ContentType = "application/json";
                request.Method = "POST";
                var blob = new RefreshBlob(this);
                var stream = request.GetRequestStream();
                serializer.Serialize(new StreamWriter(stream), blob);
                stream.Close();
                var response = request.GetResponse();
                stream = response.GetResponseStream();
                blob = serializer.Deserialize<RefreshBlob>(new JsonTextReader(new StreamReader(stream)));
                this.AccessToken = blob.AccessToken;
                this.ClientToken = blob.ClientToken;
                this.User = blob.User;
                this.SelectedProfile = blob.SelectedProfile;
                // TODO: Add profile to available profiles if need be
            }
            catch (WebException e)
            {
                var stream = e.Response.GetResponseStream();
                throw serializer.Deserialize<WebException>(new JsonTextReader(new StreamReader(stream)));
            }
        }

        [JsonProperty("availableProfiles")]
        public Profile[] AvailableProfiles { get; set; }
        [JsonProperty("selectedProfile")]
        public Profile SelectedProfile { get; set; }
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("clientToken")]
        public string ClientToken { get; set; }
        /// <summary>
        /// This is NOT the name of the player, which can be found in SelectedProfile. This is the username
        /// the user logged in with.
        /// </summary>
        [JsonIgnore]
        public string UserName { get; set; }
        [JsonIgnore]
        public string SessionId { get { return "token:" + AccessToken + ":" + SelectedProfile.Id; } }
        [JsonIgnore]
        public bool OnlineMode { get { return AccessToken != null; } }
        [JsonProperty("user")]
        public MojangUser User { get; set; }

        public static IList<ServiceStatus> ServiceStatuses()
        {
            using (WebClient wc = new WebClient())
            {
                string status = wc.DownloadString("http://status.mojang.com/check");
                JArray ja = JArray.Parse(status);
                IList<ServiceStatus> list = new List<ServiceStatus>();
                foreach (JObject item in ja)
                {
                    ServiceStatus statusItem = new ServiceStatus();
                    statusItem.Name = item.Properties().Select(p => p.Name).First();
                    statusItem.Status = item.Value<string>(statusItem.Name);
                    list.Add(statusItem);
                }
                return list;
            }
        }
        public class ServiceStatus
        {
            public ServiceStatus()
            {
            }
            public string Name { get; set; }
            public string Status { get; set; }
        }
    }
}