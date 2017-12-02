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
        /// Text of the message
        /// </summary>
        public string Message { get; }

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

        public VkMessage(Tuple<int, MessageFlags, JArray> tuple)
        {
            Message = (string)tuple.Item3[6];
            Peer = (int) tuple.Item3[3];
            Attachments = (JObject) tuple.Item3[7];
            Flags = tuple.Item2;
        }
    }
}