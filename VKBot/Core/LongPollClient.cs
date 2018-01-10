using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Core
{
    /// <summary>
    ///     Provide long poll client for VK
    /// </summary>
    public class LongPollClient
    {
        private readonly Uri _apiUrl = new Uri("https://api.vk.com/");

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Settings _settings;
        private readonly int _version = 2;
        private readonly int _wait = 25;
        private bool _isActive;
        private string _key;
        private int _mode;
        private string _serverUrl;
        private uint _ts;

        public LongPollClient(Settings settings)
        {
            _settings = settings;
            _isActive = false;
        }

        public event EventHandler<VkMessage> OnMessage;

        public async void Start()
        {
            _getLongPollServer();
            _isActive = true;
            while (_isActive)
            {
                var changes = await _sendRequest();

                if (changes != null)
                    _handleChanges(changes);
                else
                    Task.Delay(20000).Wait();
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
                _settings.Logger.Log("Error received, handling...");
                switch ((int) changes["failed"])
                {
                    case 1:
                        _ts = (uint) changes["new_ts"];
                        _settings.Logger.Log("Ts updated");
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
                        _settings.Logger.Log("Invoke OnMessage event");
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
                Query = $"access_token={_settings.Api.AccessToken.Token}&v=5.63"
            };
            try
            {
                _settings.Logger.Log($"Getting server, executing GET request {urlBuilder}");

                var responce = _httpClient.GetStringAsync(urlBuilder.Uri).Result;

                _settings.Logger.Log("Responce received, deserializing...");

                var serverParams = JObject.Parse(responce)["response"];

                _ts = (uint) serverParams["ts"];
                _serverUrl = (string) serverParams["server"];
                _key = (string) serverParams["key"];

                _settings.Logger.Log("Deserialization complete, new long poll configuration obtained");
            }
            catch (Exception e)
            {
                _settings.Logger.Log(e.ToString());
            }
        }

        private async Task<JObject> _sendRequest()
        {
#if DEBUG
            _settings.Logger.Log("Executing long poll request...");
#endif

            try
            {
                var updates = await _httpClient.GetStringAsync(
                    $"https://{_serverUrl}?act=a_check&ts={_ts}&key={_key}&version={_version}&wait={_wait}");

#if DEBUG
                _settings.Logger.Log($"Updates received {updates.Trim()}");
#endif

                var updatesDeserialized = JObject.Parse(updates);
                return updatesDeserialized;
            }
            catch (HttpRequestException e)
            {
                _settings.Logger.Log(e.Message);
                return null;
            }
        }
    }
}