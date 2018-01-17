using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Core
{
    internal class MessageHandler
    {
        private readonly Regex _messageParser;
        private readonly PluginsManager _plugins;
        private readonly Settings _settings;

        public MessageHandler(Settings settings)
        {
            _settings = settings;
            _plugins = new PluginsManager();
            _messageParser = new Regex(_buildPrefixRegex());
        }

        public void HandleMessage(VkMessage message)
        {
            if (message.Flags.HasFlag(MessageFlags.Outbox) || message.Peer == _settings.UserId) return;
            if (_messageParser.IsMatch(message.Text)) _plugins.Handle(_settings, message);
        }

        private string _buildPrefixRegex()
        {
            var sb = new StringBuilder("(?i)^(");

            var escapedSettings = _settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(") *(.+)");
            _settings.Logger.Log($"Command regex has been built: '{sb}'");

            return sb.ToString();
        }
    }
}