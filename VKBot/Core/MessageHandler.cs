using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.PluginsManaging;
using VKBot.Types;

namespace VKBot.Core
{
    internal class MessageHandler
    {
        private readonly Settings _settings;
        private readonly PluginsProvider _plugins;

        private readonly Regex _prefixRegex;

        public MessageHandler(Settings settings)
        {
            _settings = settings;
            _plugins = PluginsLoader.InitPlugins();
            _prefixRegex = new Regex(_buildPrefixRegex());
        }

        public void HandleMessage(Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            if (!_prefixRegex.IsMatch(message) || tuple.Item2.HasFlag(MessageFlags.Outbox) ||
                tuple.Item1 == _settings.UserId) return;

            _plugins.Handle(_settings, tuple);
        }

        private string _buildPrefixRegex()
        {
            var sb = new StringBuilder("^(");

            var escapedSettings = _settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(")");

            return sb.ToString();
        }
    }
}