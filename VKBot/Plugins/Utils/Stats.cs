using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Services;
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

        public Task Handle(VkMessage message, Vkontakte api, ILogger logger)
        {
            var peer = message.Peer;
            var result = $"Бот работает уже {DateTime.Now - _start:c}";
            return api.Messages.Send(peerId: peer, message: result);
        }
    }
}