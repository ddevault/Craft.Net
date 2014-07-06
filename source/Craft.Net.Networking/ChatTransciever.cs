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

    public enum ChatActionType
    {
        NONE,OPEN_URL,
        OPEN_FILE,
        RUN_COMMAND,
        SUGGEST_COMMAND,
    }

    public enum ChatHoverActionType
    {
        NONE,
        SHOW_TEXT,
        SHOW_ACHIEVEMENT,
        SHOW_ITEM,
    }

    public class ChatMessage
    {
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

        public IList<ChatMessage> SubMessages { get; protected set; }

        public ChatMessage(string Message)
        {
            RawMessage = Message;
            try
            {
                Init(JObject.Parse(Message));
            } catch (Newtonsoft.Json.JsonReaderException)
            {
                //If Newtonsoft.Json couldn't parse it, that means we got something that isn't a new-format chat message.
                // (Or there's a bug in the server)
                //So we're just gona set the text content and call it a day.
                Text = Message;
            }
        }

        public ChatMessage(string Message, ChatColor color)
        {
            this.Text = Message;
            this.Color = color;
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
                var eventSpec = obj["clickEvent"];
                var action = eventSpec.Value<string>("action").ToUpper();
                this.ChatActionValue = eventSpec.Value<string>("value");
                ChatActionType c;
                if (!ChatActionType.TryParse(action, out c))
                    this.Action = ChatActionType.NONE;
                else
                    this.Action = c;
            }
            else
            {
                this.Action = ChatActionType.NONE;
            }
            if (obj["hoverEvent"] != null)
            {
                var eventSpec = obj["hoverEvent"];
                var action = eventSpec.Value<string>("action").ToUpper();
                this.ChatHoverActionValue = eventSpec.Value<string>("value");
                ChatHoverActionType c;
                if (!ChatHoverActionType.TryParse(action, out c))
                    this.HoverAction = ChatHoverActionType.NONE;
                else
                    this.HoverAction = c;
            }
            else
            {
                this.HoverAction = ChatHoverActionType.NONE;
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
                        this.SubMessages.Add(new ChatMessage((string)((JValue)o).Value));
                    }
                    else
                    {
                        this.SubMessages.Add(new ChatMessage((JObject)o));
                    }
                }
            }
        }


        public string FullText()
        {
            var text = this.Text;
            if (SubMessages != null)
            {
                foreach (ChatMessage c in SubMessages)
                {
                    text += c.FullText();
                }
            }
            return text;
        }

        public JObject AsJObject()
        {
            JObject self = new JObject();
            JArray extras = null;
            if (SubMessages != null)
            {
                extras = new JArray();

                foreach (ChatMessage c in SubMessages)
                {
                    extras.Add(c.AsJObject());
                }
            }
            // 'extra' may not be empty if it is present.
            if (extras != null)
            {
                self.Add(new JProperty("extra", extras));
            }
            self.Add(new JProperty("text", this.Text));
            self.Add(new JProperty("bold", this.Bold));
            self.Add(new JProperty("strikethrough", this.StrikeThrough));
            self.Add(new JProperty("obfuscated", this.Obfuscated));
            self.Add(new JProperty("underlined", this.Underlined));
            self.Add(new JProperty("color", this.Color.ToString().ToLower()));
            if (this.Action != ChatActionType.NONE)
            {
                self.Add(new JProperty("clickEvent",
                                       new JObject(new JProperty("action", this.Action.ToString().ToLower())
                                                  , new JProperty("value", this.ChatActionValue))
                )
                );
            }
            if (this.HoverAction != ChatHoverActionType.NONE)
            {
                self.Add(new JProperty("hoverEvent",
                                       new JObject(new JProperty("action", this.HoverAction.ToString().ToLower())
                                                 , new JProperty("value", this.ChatHoverActionValue))
                )
                );
            }
            return self;
        }

        public string ToJson()
        {
            return AsJObject().ToString();
        }

        public override string ToString()
        {
            return this.FullText();
        }
    }
}

