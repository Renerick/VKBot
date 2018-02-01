﻿using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;

namespace VKBot.Types
{
    /// <summary>
    ///     This class contains data from long poll server
    /// </summary>
    public class VkMessage
    {
        /// <summary>
        /// </summary>
        private static Regex _commandRegex;

        public VkMessage(int messageId, string text, int peer, JObject attachments, MessageFlags flags)
        {
            MessageId = messageId;
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Peer = peer;
            Attachments = attachments;
            Flags = flags;
        }

        public static Regex CommandRegex
        {
            get => _commandRegex;
            set
            {
                if (_commandRegex == null)
                    _commandRegex = value;
                else
                    throw new Exception("Command regex can't be redefined");
            }
        }

        /// <summary>
        ///     Id of message
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
        public string Command => _commandRegex.Match(Text).Groups[2].Value;

        /// <summary>
        ///     Check if message is a bot command
        /// </summary>
        public bool IsCommand => _commandRegex.IsMatch(Text);
    }
}