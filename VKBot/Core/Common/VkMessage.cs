using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;

namespace VKBot.Core.Common
{
    /// <summary>
    ///     This class contains message data received from a long poll server
    /// </summary>
    public class VkMessage
    {
        private readonly Regex _commandRegex;

        public VkMessage(int messageId, string text, int peer, JObject attachments, MessageFlags flags, Regex commandRegex)
        {
            MessageId = messageId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Peer = peer;
            Attachments = attachments;
            Flags = flags;
            _commandRegex = commandRegex;
        }

        /// <summary>
        ///     Id of the message
        /// </summary>
        public int MessageId { get; }

        /// <summary>
        ///     Text of the message
        /// </summary>
        public string Text { get; }

        /// <summary>
        ///     Peer id
        /// </summary>
        public int Peer { get; }

        /// <summary>
        ///     Attachments in the message
        /// </summary>
        public JObject Attachments { get; }

        /// <summary>
        ///     Message flags
        /// </summary>
        public MessageFlags Flags { get; }

        public string Prefix => _commandRegex.Match(Text).Groups[1].Value;
        public string Body => _commandRegex.Match(Text).Groups[2].Value;

        /// <summary>
        ///     Check if the message is a bot command
        /// </summary>
        public bool IsCommand => _commandRegex.IsMatch(Text);
    }
}