using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Core
{
    internal class MessageHandler
    {
        private readonly Settings _settings;
        private readonly PluginsManager _plugins;

        private readonly Regex _messageParser;

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
            var sb = new StringBuilder("^(");

            var escapedSettings = _settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(") *(.+)");

            return sb.ToString();
        }
    }
}