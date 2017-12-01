using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Plugins;
using VKBot.Types;

namespace VKBotPlugins
{
    [VkBotPlugin]
    public class Stats : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"stats", "статы"};

        private readonly DateTime _start;

        public Stats()
        {
            _start = DateTime.Now;
        }
        
        public Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var result = $"Бот работает уже {DateTime.Now - _start:c}";
            return settings.Api.Messages.Send(peerId: peer, message: result);
        }
    }
}