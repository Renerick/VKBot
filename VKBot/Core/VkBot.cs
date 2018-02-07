using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Services;
using VKBot.Core.Common;
using VKBot.Core.Services;

namespace VKBot.Core
{
    public class VkBot
    {
        private readonly VkLongPollClient _longPollClient;

        private readonly PluginsService _plugins;

        private readonly ServiceContext _services;

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

            Settings.UserId = loginData.UserId;
            var api = new Vkontakte(loginData.AppId, loginData.AppSecret, logger, parseJson: ParseJson.FromStream);

            // login
            if (loginData.AccessToken != null)
            {
                var accessToken = AccessToken.FromString(loginData.AccessToken, loginData.UserId);
                _services.ApiService.AccessToken = accessToken;
            }
            else
            {
                throw new NotImplementedException("There is no auth through login and password now");
            }

            _longPollClient = new VkLongPollClient(_services.ApiService.AccessToken.Token, _services);
            
            _services = new ServiceContext(logger, api);

            VkMessage.CommandRegex = _buildPrefixRegex();
            _plugins = new PluginsService(_services);
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
                _services.Logger.Log($"Exception in message handler\n{exception}");
            }
        }

        private Regex _buildPrefixRegex()
        {
            var sb = new StringBuilder("(?i)^(");

            var escapedSettings = Settings.Prefixes.Select(Regex.Escape);
            sb.Append(string.Join("|", escapedSettings)).Append(") *(.+)");
            _services.Logger.Log($"Command regex has been built: '{sb}'");

            return new Regex(sb.ToString());
        }

        private void _handle(VkMessage message)
        {
            if (message.Flags.HasFlag(MessageFlags.Outbox) || message.Peer == Settings.UserId) return;
            if (message.IsCommand) _plugins.HandleMessage(message);
        }
    }
}