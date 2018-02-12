using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Core.Common;

namespace VKBot.Core.Services
{
    public class VkMessageFactory
    {
        private Regex _commandRegex;

        public VkMessageFactory(Regex commandRegex)
        {
            _commandRegex = commandRegex;
        }

        public VkMessage Build(int messageId, string text, int peer, JObject attachments, MessageFlags flags)
        {
            return new VkMessage(messageId, text, peer, attachments, flags, _commandRegex);
        }
    }
}