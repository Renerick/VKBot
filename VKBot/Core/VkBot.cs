using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Services;
using VKBot.PluginsManaging;
using VKBot.Types;

namespace VKBot.Core
{
    public class VkBot
    {
        public Settings Settings { get; }

        private readonly MessageHandler _messageHandler;

        public VkBot(LoginData loginData, Settings settings, ILogger logger = null)
        {
            Settings = settings;
            
            Settings.UserId = loginData.UserId;
            Settings.Api = new Vkontakte(loginData.AppId, loginData.AppSecret, logger, parseJson: ParseJson.FromStream);

            if (loginData.AccessToken != null)
            {
                var accessToken = AccessToken.FromString(loginData.AccessToken, loginData.UserId);
                Settings.Api.AccessToken = accessToken;
            }
            else
            {
                throw new NotImplementedException("There is no auth through login and password now");
            }
            
            _messageHandler = new MessageHandler(Settings, new PluginsManager());
        }


        public void StartBot()
        {
            StartLongPoll();

            Task.Delay(-1).Wait();
        }

        private void StartLongPoll()
        {
            var longPollParams = Settings.Api.Messages.GetLongPollServer().Result;
            var longPollClient = Settings.Api.StartLongPollClient(longPollParams.Server, longPollParams.Key, longPollParams.Ts)
                .Result;

            longPollClient.AddMessageEvent += HandleMessage;
            longPollClient.LongPollFailureReceived += HandleError;
        }

        private void HandleMessage(object o, Tuple<int, MessageFlags, JArray> tuple)
        {
            _messageHandler.HandleMessage(tuple);
        }

        private void HandleError(object client, int i)
        {
            StartLongPoll();
        }
    }
}