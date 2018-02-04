﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;
using VKBot.Core.Common.Services;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class RollDice : IPlugin
    {
        private readonly Random _random = new Random();
        public IEnumerable<string> Commands { get; } = new[] {"roll", "кубик", "dice", "random", "рандом"};

        public Task Handle(VkMessage message)
        {
            var messageTokens = message.Body.Split();

            int limit;

            if (messageTokens.Length >= 2)
            {
                if (!int.TryParse(messageTokens[1], out limit))
                    return MessageService.Perform.Send(peerId: message.Peer, message: "Не могу определить диапазон");
            }
            else
            {
                limit = 100;
            }

            return MessageService.Perform.Send(peerId: message.Peer, message: _random.Next(limit).ToString());
        }
    }
}