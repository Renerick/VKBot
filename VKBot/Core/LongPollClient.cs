using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.Types.Messages;
using VKBot.Types;

namespace VKBot.Core
{
    /// <summary>
    /// Provide long poll client for VK
    /// </summary>
    public class LongPollClient
    {
        private string _serverUrl;
        private string _key;
        private uint _ts;
        private int _wait = 25;
        private int _mode;
        private readonly int _version = 2;
        private bool _isActive;

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Uri _apiUrl = new Uri("https://api.vk.com/");

        private readonly Settings _settings;

        public event EventHandler<Message> OnMessage;

        public LongPollClient(Settings settings)
        {
            _settings = settings;
            _isActive = false;
            _getLongPollServer();
        }

        public async void Start()
        {
            _isActive = true;
            while (_isActive)
            {
                _sendRequest();
            }
        }

        private void _getLongPollServer()
        {
            var urlBuilder = new UriBuilder(_apiUrl)
            {
                Path = "method/messages.getLongPollServer",
                Query = $"access_token={_settings.Api.AccessToken.Token}&v=5.63"
            };

            _settings.Logger.Log($"Executing GET request {urlBuilder}");

            var responce = _httpClient.GetStringAsync(urlBuilder.Uri).Result;

            _settings.Logger.Log($"Responce received, deserializing...");

            var serverParams = JObject.Parse(responce)["response"];
            
            _ts = (uint) serverParams["ts"];
            _serverUrl = (string) serverParams["server"];
            _key = (string) serverParams["key"];

            _settings.Logger.Log("Deserializing complete, long poll configured");
        }

        private void _sendRequest()
        {
            
            _settings.Logger.Log($"Executing long poll request...");

            var updates = _httpClient.GetStringAsync(
                $"https://{_serverUrl}?act=a_check&ts={_ts}&key={_key}&version=2&wait={_wait}").Result;

            _settings.Logger.Log($"Updates received {updates.Trim()}");

            var updatesDeserialized = JObject.Parse(updates);
            _ts = (uint) updatesDeserialized["ts"];
        }
    }
}