using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VkLibrary.Core.LongPolling;
using VKBot.PluginsManaging;
using VKBot.Types;

namespace VKBot
{
    public class VkBot
    {
        private readonly int _userId;
        private readonly Vkontakte _api;
        private readonly MessageHandler _messageHandler;

        public VkBot(LoginData loginData, Settings settings, Action<object> logger = null)
        {
            _api = new Vkontakte(loginData.AppId, JsonParsingType.UseStream, logger: logger);

            if (loginData.AccessToken != null)
            {
                var accessToken = AccessToken.FromString(loginData.AccessToken, loginData.UserId);
                _api.AccessToken = accessToken;
            }
            else
            {
                throw new NotImplementedException("There is no auth through login and password now");
            }
            
            _messageHandler = new MessageHandler(_api, new PluginsManager(), loginData.UserId);
            Settings = settings;
        }

        public Settings Settings { get; }

        public void StartBot()
        {
            StartLongPoll();

            Task.Delay(-1).Wait();
        }

        private void StartLongPoll()
        {
            var longPollParams = _api.Messages.GetLongPollServer().Result;
            var longPollClient = _api.StartLongPollClient(longPollParams.Server, longPollParams.Key, longPollParams.Ts)
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