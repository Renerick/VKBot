using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;

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

        public Task Handle(Settings settings, VkMessage message)
        {
            var peer = message.Peer;
            var result = $"Бот работает уже {DateTime.Now - _start:c}";
            return settings.Api.Messages.Send(peerId: peer, message: result);
        }
    }
}