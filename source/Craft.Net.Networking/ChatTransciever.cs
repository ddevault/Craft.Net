using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Craft.Net.Common;

namespace Craft.Net.Networking
{
    public enum ChatColor
    {
        BLACK,
        DARK_BLUE,
        DARK_GREEN,
        DARK_AQUA,
        DARK_RED,
        DARK_PURPLE,
        GOLD,
        GRAY,
        DARK_GRAY,
        BLUE,
        GREEN,
        AQUA,
        RED,
        LIGHT_PURPLE,
        YELLOW,
        WHITE,
        OBFUSCATED,
        BOLD,
        STRIKETHROUGH,
        UNDERLINE,
        ITALIC,
        RESET,
    }

    //Lowercase because that's the way vanilla sends the "clickEvent" field.
    //In fact, the vanilla server will only send the message if it's lowercase.
    public enum ChatActionType
    {
        open_url,
        open_file,
        run_command,
        suggest_command,
        none
    }
    
    //Lowercase because that's the way vanilla sends the "hoverEvent" field.
    //In fact, the vanilla server will only send the message if it's lowercase.
    public enum ChatHoverActionType
    {
        show_text,
        show_achievement,
        show_item,
        none
    }

    public class ChatMessage
    {
        #region Properties

        public string RawMessage { get; private set; }

        public string Text { get; protected set; }

        public ChatColor Color { get; protected set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Craft.Net.Networking.ChatMessage"/> is bolded.
        /// Can be overriden by submessages with a different value.
        /// </summary>
        public bool Bold { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Craft.Net.Networking.ChatMessage"/> is italicized.
        /// Can be overriden by submessages with a different value.
        /// </summary>
        public bool Italic { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Craft.Net.Networking.ChatMessage"/> is underlined.
        /// Can be overriden by submessages with a different value.
        /// </summary>
        public bool Underlined { get; protected set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Craft.Net.Networking.ChatMessage"/> is struck through.
        /// Can be overriden by submessages with a different value.
        /// </summary>
        public bool StrikeThrough { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Craft.Net.Networking.ChatMessage"/> is obfuscated.
        /// "Obfuscated" in this context means the client will display this message segment with random characters
        /// instead of the actual text.
        /// Can be overriden by submessages with a different value.
        /// </summary>
        public bool Obfuscated { get; protected set; }

        public ChatActionType Action { get; protected set; }

        /// <summary>
        /// Gets or sets the chat action value.
        /// Should only be set if Action != ChatACtionType.none
        /// </summary>
        public string ChatActionValue { get; protected set; }

        public ChatHoverActionType HoverAction { get; protected set; }

        public string ChatHoverActionValue { get; protected set; }

        public bool IsCommand { get; private set; }

        public IList<ChatMessage> SubMessages { get; protected set; }

        #endregion

        #region Constructors

        public ChatMessage(string Message)
        {
            RawMessage = Message;
            try
            {
                Init(JObject.Parse(Message));
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                //If Newtonsoft.Json couldn't parse it, that means we got something that isn't a new-format chat message.
                // (Or there's a bug in the server)
                //So we're just gona set the text content and call it a day.
                Text = Message;
                if (Text.StartsWith("/"))
                    this.IsCommand = true;
            }
        }

        private ChatMessage(JObject obj)
        {
            Init(obj);
        }

        private void Init(JObject obj)
        {
            if (obj["text"] != null)
            {
                Text = obj.Value<string>("text");
            }
            // Try to parse enumerations
            if (obj["color"] != null)
            {
                var color = obj.Value<string>("color").ToUpper();
                ChatColor c;
                if (!ChatColor.TryParse(color, out c))
                    this.Color = ChatColor.RESET;
                else
                    this.Color = c;
            }
            else
            {
                this.Color = ChatColor.RESET;
            }
            if (obj["clickEvent"] != null)
            {
                var action = obj.Value<String>("clickEvent");
                ChatActionType c;
                if (!ChatActionType.TryParse(action, out c))
                    this.Action = ChatActionType.none;
                else
                    this.Action = c;
            }
            else
            {
                this.Action = ChatActionType.none;
            }
            if (obj["hoverEvent"] != null)
            {
                var hover = obj.Value<string>("hoverEvent");
                ChatHoverActionType c;
                if (!ChatHoverActionType.TryParse(hover, out c))
                    this.HoverAction = ChatHoverActionType.none;
                else
                    this.HoverAction = c;
            }
            else
            {
                this.HoverAction = ChatHoverActionType.none;
            }
            //Parse booleans
            if (obj["bold"] != null)
            {
                Bold = obj.Value<bool>("bold");
            }
            if (obj["italic"] != null)
            {
                Italic = obj.Value<bool>("italic");
            }
            if (obj["underlined"] != null)
            {
                Underlined = obj.Value<bool>("underlined");
            }
            if (obj["strikethrough"] != null)
            {
                StrikeThrough = obj.Value<bool>("strikethrough");
            }
            if (obj["obfuscated"] != null)
            {
                Obfuscated = obj.Value<bool>("obfuscated");
            }

            if (obj["extra"] != null)
            {
                this.SubMessages = new List<ChatMessage>();
                foreach (object o in (JArray)obj["extra"])
                {
                    if (o.GetType() == typeof(JValue))
                    {
                        Console.WriteLine(((JValue)o).Type);
                        this.SubMessages.Add(new ChatMessage((string)((JValue)o).Value));
                    }
                    else
                    {
                        this.SubMessages.Add(new ChatMessage((JObject)o));
                    }
                }
            }

            #endregion
        }

        #region Public methods

        public string FullText()
        {
            //TODO
            return "";
        }

        public override string ToString()
        {
            string extras = "[";
            if (SubMessages != null)
            {
                foreach (ChatMessage c in SubMessages)
                {
                    extras += c.ToString();
                    extras += ",";
                }
                extras = extras.Substring(0, extras.Length - 1); // Strip off last comma
            }
            extras += "]";
            //{}'s are added after format to avoid confusing the formating engine
            return "{" + string.Format(
                "\"text\":\"{0}\",\"bold\":{1},\"italic\":{2},\"underlined\":{3},\"strikethrough\":{4},\"obfuscated\":{5}" +
                "\"color\":{6},\"clickEvent\":{{\"action\":\"{7}\",\"value\":\"{8}\"}},\"hoverEvent\":{{\"action\":\"{9}\",\"value\":\"{10}\"}}," +
                "\"extra\":{11}",
                Text, Bold, Italic, Underlined, StrikeThrough, Obfuscated, Color,
                ((Action != ChatActionType.none) ? Action.ToString() : ""), ChatActionValue,
                ((HoverAction != ChatHoverActionType.none) ? HoverAction.ToString() : ""), ChatHoverActionValue, extras
                ) + "}";
        }

        #endregion
    }
}

