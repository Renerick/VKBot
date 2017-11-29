using System;
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

        private Regex _prefixValidator;
        
        public MessageHandler(Settings settings)
        {
            _settings = settings;
            _plugins = PluginsLoader.InitPlugins();
            _prefixValidator = new Regex(_buildValidatorRegex());
        }

        public async void HandleMessage(Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            if (!_prefixValidator.IsMatch(message) || tuple.Item2.HasFlag(MessageFlags.Outbox) ||
                tuple.Item1 == _settings.UserId) return;
            
            await _plugins.Handle(_settings, tuple);
        }

        private string _buildValidatorRegex()
        {
            var sb = new StringBuilder("^(");
            sb.Append(string.Join("|", _settings.Prefixes)).Append(")");
            return sb.ToString();
        }
    }
}