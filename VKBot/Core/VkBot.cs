using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Services;
using VKBot.Core.Services;
using VKBot.Types;

namespace VKBot.Core
{
    public class VkBot
    {
        private readonly VkLongPollClient _longPollClient;

        private readonly PluginsService _plugins;

        /// <summary>
        ///     Primary constructor, initialize bot settings
        /// </summary>
        /// <param name="loginData">Data for login to VK</param>
        /// <param name="settings">Bot settings object</param>
        /// <param name="logger">Logger object</param>
        /// <exception cref="NotImplementedException">
        ///     Bot can't use login and password for authentification now, so there is the
        ///     exception
        /// </exception>
        public VkBot(LoginData loginData, Settings settings, ILogger logger = null)
        {
            Settings = settings;
            LoggerService.Logger = logger;

            Settings.UserId = loginData.UserId;
            Settings.Api = new Vkontakte(loginData.AppId, loginData.AppSecret, logger, parseJson: ParseJson.FromStream);

            // login
            if (loginData.AccessToken != null)
            {
                var accessToken = AccessToken.FromString(loginData.AccessToken, loginData.UserId);
                Settings.Api.AccessToken = accessToken;
            }
            else
            {
                throw new NotImplementedException("There is no auth through login and password now");
            }

            _longPollClient = new VkLongPollClient(Settings.Api.AccessToken.Token);

            VkMessage.CommandRegex = _buildPrefixRegex();
            _plugins = new PluginsService();
        }

        private Settings Settings { get; }

        /// <summary>
        ///     Start bot's long poll message receiving
        /// </summary>
        public void StartBot()
        {
            StartLongPoll();

            Task.Delay(-1).Wait();
        }

        private void StartLongPoll()
        {
            _longPollClient.OnMessage += _handleMessage;
            _longPollClient.Start();
        }

        private void _handleMessage(object o, MessageEventArgs e)
        {
            try
            {
                _handle(e.Message);
            }
            catch (Exception exception)
            {
                LoggerService.Logger.Log($"Exception in message handler\n{exception}");
            }
        }

        private Regex _buildPrefixRegex()
        {
            var sb = new StringBuilder("(?i)^(");

            var escapedSettings = Settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(") *(.+)");
            LoggerService.Logger.Log($"Command regex has been built: '{sb}'");

            return new Regex(sb.ToString());
        }

        private void _handle(VkMessage message)
        {
            if (message.Flags.HasFlag(MessageFlags.Outbox) || message.Peer == Settings.UserId) return;
            if (message.IsCommand) _plugins.Handle(Settings, message);
        }
    }
}