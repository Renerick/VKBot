using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot
{
    public class VkBot
    {
        private readonly int _userId;
        private readonly Vkontakte _api;

        public VkBot(LoginData loginData, Settings settings, Action<object> logger = null)
        {
            _userId = loginData.UserId;
            Settings = settings;

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

        private async void HandleMessage(object o, Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            if (!message.StartsWith("!") || tuple.Item2.HasFlag(MessageFlags.Outbox) ||
                tuple.Item1 == _userId) return;

            await _api.Messages.Send(peerId: peer, message: message + "!");
        }

        private void HandleError(object client, int i)
        {
            StartLongPoll();
        }
    }
}