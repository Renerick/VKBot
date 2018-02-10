using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Services;
using VKBot.Core.Common;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class RollDice : IPlugin
    {
        private readonly Random _random = new Random();
        public IEnumerable<string> Commands { get; } = new[] {"roll", "кубик", "dice", "random", "рандом"};

        public Task Handle(VkMessage message, Vkontakte api, ILogger logger)
        {
            var messageTokens = message.Body.Split();

            int limit;

            if (messageTokens.Length >= 2)
            {
                if (!int.TryParse(messageTokens[1], out limit))
                    return api.Messages.Send(peerId: message.Peer, message: "Не могу определить диапазон");
            }
            else
            {
                limit = 100;
            }

            return api.Messages.Send(peerId: message.Peer, message: _random.Next(limit).ToString());
        }
    }
}