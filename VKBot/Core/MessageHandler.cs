using System;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.LongPolling;
using VkLibrary.Core.Types.Messages;
using VKBot.PluginsManaging;
using VKBot.Types;

namespace VKBot.Core
{
    public class MessageHandler
    {
        private readonly Settings _settings;
        private readonly Plugins _plugins;
        
        public MessageHandler(Settings settings, PluginsManager pluginsManager)
        {
            _settings = settings;
            _plugins = pluginsManager.InitPlugins();
        }

        public async void HandleMessage(Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            if (!message.StartsWith("!") || tuple.Item2.HasFlag(MessageFlags.Outbox) ||
                tuple.Item1 == _settings.UserId) return;
            
            await _plugins.Handle(_settings, tuple);
        } 
    }
}