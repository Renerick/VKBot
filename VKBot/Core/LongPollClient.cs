using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Services;
using VKBot.Types;

namespace VKBot.Core
{
    /// <summary>
    ///     Provide long poll client for VK
    /// </summary>
    public class VkLongPollClient
    {
        private readonly Uri _apiUrl = new Uri("https://api.vk.com/");

        private readonly HttpClient _httpClient;

        private readonly string _apiAccessToken;
        private ILogger _logger;
        private readonly int _version = 2;
        private readonly int _wait = 25;
        private bool _isActive;
        private string _key;
        private string _serverUrl;
        private uint _ts;

        public VkLongPollClient(string apiAccessToken, ILogger logger)
        {
            _apiAccessToken = apiAccessToken;
            _logger = logger;
            _httpClient = new HttpClient();
        }

        public event EventHandler<VkMessage> OnMessage;

        public async void Start()
        {
            _getLongPollServer();
            _isActive = true;
            while (_isActive)
            {
                try
                {
                    var changes = await _sendRequest();
                    _handleChanges(changes);
                }
                catch (Exception e)
                {
                    _logger.Log($"Exception in long poll request, trying again...\n{e}");
                    Task.Delay(2000).Wait();
                }
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
                _logger.Log("Error received, handling...");
                switch ((int) changes["failed"])
                {
                    case 1:
                        _ts = (uint) changes["new_ts"];
                        _logger.Log("Ts updated");
                        break;
                    case 2:
                    case 3:
                        _getLongPollServer();
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
                        var message = new VkMessage(id, text, peer, attachments, flags);
                        _logger.Log("Invoke OnMessage event");
                        OnMessage?.Invoke(this, message);
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
                _logger.Log($"Getting server, executing GET request {urlBuilder}");

                var responce = _httpClient.GetStringAsync(urlBuilder.Uri).Result;

                _logger.Log("Responce received, deserializing...");

                var serverParams = JObject.Parse(responce)["response"];

                _ts = (uint) serverParams["ts"];
                _serverUrl = (string) serverParams["server"];
                _key = (string) serverParams["key"];

                _logger.Log("Deserialization complete, new long poll configuration obtained");
            }
            catch (Exception e)
            {
                _logger.Log(e.ToString());
            }
        }

        private async Task<JObject> _sendRequest()
        {
#if DEBUG
            _logger.Log("Executing long poll request...");
#endif

            var updates = await _httpClient.GetStringAsync(
                $"https://{_serverUrl}?act=a_check&ts={_ts}&key={_key}&version={_version}&wait={_wait}");

#if DEBUG
            _logger.Log($"Updates received {updates.Trim()}");
#endif

            var updatesDeserialized = JObject.Parse(updates);
            return updatesDeserialized;
        }
    }
}