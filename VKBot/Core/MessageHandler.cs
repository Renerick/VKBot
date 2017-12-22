using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Types.Messages;
using VKBot.PluginsManaging;
using VKBot.Types;

namespace VKBot.Core
{
    internal class MessageHandler
    {
        private readonly Settings _settings;
        private readonly PluginsProvider _plugins;

        private readonly Regex _messageParser;

        public MessageHandler(Settings settings)
        {
            _settings = settings;
            _plugins = PluginsLoader.InitPlugins();
            _messageParser = new Regex(_buildPrefixRegex());
        }

        public void HandleMessage(Message message)
        {
            
        }

        private string _buildPrefixRegex()
        {
            var sb = new StringBuilder("^(");

            var escapedSettings = _settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(") *(.+)");

            return sb.ToString();
        }
    }
}