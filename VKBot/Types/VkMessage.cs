using System;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;

namespace VKBot.Types
{
    /// <summary>
    /// This class contains data from long poll server
    /// </summary>
    public class VkMessage
    {
        /// <summary>
        /// Id of message
        /// </summary>
        public int MessageId { get; }
        
        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Peer id
        /// </summary>
        public int Peer { get; }

        /// <summary>
        /// Attachments in the message
        /// </summary>
        public JObject Attachments { get; }
        
        /// <summary>
        /// Message flags
        /// </summary>
        public MessageFlags Flags { get; }

        public VkMessage(int messageId, string text, int peer, JObject attachments, MessageFlags flags)
        {
            MessageId = messageId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Peer = peer;
            Attachments = attachments;
            Flags = flags;
        }
    }
}