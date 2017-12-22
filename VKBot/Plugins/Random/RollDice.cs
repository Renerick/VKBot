﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Types;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class RollDice : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"roll", "кубик", "dice", "random", "рандом"};

        private readonly System.Random _random = new System.Random();

        public Task Handle(Settings settings, VkMessage message)
        {
            var messageTokens = message.Text.Split();

            int limit;

            if (messageTokens.Length >= 3)
            {
                var parseSuccess = int.TryParse(messageTokens[2], out limit);
                if (!parseSuccess)
                    return settings.Api.Messages.Send(peerId: message.Peer, message: "Не могу определить диапазон");
            }
            else
            {
                limit = 100;
            }
            return settings.Api.Messages.Send(peerId: message.Peer, message: _random.Next(limit).ToString());
        }
    }
}