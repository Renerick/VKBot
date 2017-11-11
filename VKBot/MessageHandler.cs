using System;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.LongPolling;
using VKBot.PluginsManaging;

namespace VKBot
{
    public class MessageHandler
    {
        private readonly Vkontakte _api;
        private readonly PluginsManager _pluginsManager;
        private readonly int _userId;
        
        public MessageHandler(Vkontakte api, PluginsManager pluginsManager, int userId)
        {
            _api = api;
            _pluginsManager = pluginsManager;
            _userId = userId;
        }

        public async void HandleMessage(Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            if (!message.StartsWith("!") || tuple.Item2.HasFlag(MessageFlags.Outbox) ||
                tuple.Item1 == _userId) return;

            await _api.Messages.Send(peerId: peer, message: message + "!");
        } 
    }
}