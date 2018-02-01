using System;
using VKBot.Types;

namespace VKBot.Core
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(VkMessage message)
        {
            Message = message;
        }

        public VkMessage Message { get; }
    }
}