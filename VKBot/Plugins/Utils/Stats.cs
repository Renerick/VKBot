using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;
using VKBot.Core.Services;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class Stats : IPlugin
    {
        private readonly DateTime _start;

        public Stats()
        {
            _start = DateTime.Now;
        }

        public IEnumerable<string> Commands { get; } = new[] {"stats", "статы"};

        public Task Handle(VkMessage message, ServiceContext services)
        {
            var peer = message.Peer;
            var result = $"Бот работает уже {DateTime.Now - _start:c}";
            return services.ApiService.Messages.Send(peerId: peer, message: result);
        }
    }
}