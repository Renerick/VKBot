using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Core.Services;

namespace VKBot.Core
{
    /// <summary>
    ///     Provide long poll client for VK
    /// </summary>
    public class VkLongPollClient
    {
        private readonly string _apiAccessToken;
        private readonly Uri _apiUrl = new Uri("https://api.vk.com/");
        private readonly ServiceContext _services;

        private readonly HttpClient _httpClient;
        private readonly int _version = 2;
        private readonly int _wait = 25;
        private bool _isActive;
        private string _key;
        private string _serverUrl;
        private uint _ts;

        public VkLongPollClient(string apiAccessToken, ServiceContext services)
        {
            _apiAccessToken = apiAccessToken;
            _httpClient = new HttpClient();
            _services = services;
        }

        public event EventHandler<MessageEventArgs> OnMessage;

        public async void Start()
        {
            _getLongPollServer();
            _isActive = true;
            while (_isActive)
                try
                {
                    var changes = await _sendRequest();
                    _handleChanges(changes);
                }
                catch (Exception e)
                {
                    _services.Logger.Log($"Received an exception during a long poll request, trying again...\n{e}");
                    Task.Delay(2000).Wait();
                }
        }

        public void Stop()
        {
            _isActive = false;
        }

        private void _handleChanges(JObject changes)
        {
            if (changes["failed"] != null)
            {
                _services.Logger.Log("Error received, handling...");
                var code = (int) changes["failed"];
                switch (code)
                {
                    case 1:
                        _ts = (uint) changes["new_ts"];
                        _services.Logger.Log("Ts updated");
                        break;
                    case 2:
                    case 3:
                        _getLongPollServer();
                        break;
                    default:
                        _services.Logger.Log($"Unknown error code has been detected in a long poll response: {code}");
                        break;
                }

                return;
            }

            _ts = (uint) changes["ts"];
            var updates = (JArray) changes["updates"];

            foreach (var update in updates)
                switch ((int) update[0])
                {
                    case 4:
                    {
                        var updateArr = (JArray) update;
                        var id = (int) updateArr[1];
                        var text = (string) updateArr[5];
                        var peer = (int) updateArr[3];

                        JObject attachments = null;
                        if (updateArr.Count > 6)
                            attachments = (JObject) update[6];

                        var flags = (MessageFlags) (int) update[2];
                        var message = _services.MessageFactory.Build(id, text, peer, attachments, flags);
                        _services.Logger.Log("Invoke OnMessage event");
                        _onMessage(new MessageEventArgs(message));
                        break;
                    }
                }
        }


        private void _getLongPollServer()
        {
            var urlBuilder = new UriBuilder(_apiUrl)
            {
                Path = "method/messages.getLongPollServer",
                Query = $"access_token={_apiAccessToken}&v=5.63"
            };
            try
            {
                _services.Logger.Log($"Getting server, executing GET request {urlBuilder}");

                var responce = _httpClient.GetStringAsync(urlBuilder.Uri).Result;

                _services.Logger.Log("Responce received, deserializing...");

                var serverParams = JObject.Parse(responce)["response"];

                _ts = (uint) serverParams["ts"];
                _serverUrl = (string) serverParams["server"];
                _key = (string) serverParams["key"];

                _services.Logger.Log("Deserialization complete, new long poll configuration obtained");
            }
            catch (Exception e)
            {
                _services.Logger.Log(e.ToString());
            }
        }

        private void _onMessage(MessageEventArgs message)
        {
            var h = OnMessage;
            h?.Invoke(this, message);
        }

        private async Task<JObject> _sendRequest()
        {
#if DEBUG
            _services.Logger.Log("Executing long poll request...");
#endif

            var updates = await _httpClient.GetStringAsync(
                $"https://{_serverUrl}?act=a_check&ts={_ts}&key={_key}&version={_version}&wait={_wait}");

#if DEBUG
            _services.Logger.Log($"Updates received {updates.Trim()}");
#endif

            var updatesDeserialized = JObject.Parse(updates);
            return updatesDeserialized;
        }
    }
}