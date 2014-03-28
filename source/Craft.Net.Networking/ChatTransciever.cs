using System;
using Newtonsoft.Json.Linq;

namespace Craft.Net.Networking
{
    public class ChatMessage
    {
        #region Properties

        protected string _rawMessage;
        public string RawMessage
        {
            get { return _rawMessage; }
        }

        public string MessageText
        {
            get { return ""; /*todo*/ }
            set { /*todo*/ }
        }

        public bool IsCommand { get; private set; }

        #endregion

        #region Constructors

        public ChatMessage(string Message) : this (JObject.Parse(Message)) { }

        private ChatMessage (JObject obj)
        {

        }

        #endregion

        #region Public methods
        public string FullText()
        {
            //TODO
            return "";
        }
        #endregion
    }
}

