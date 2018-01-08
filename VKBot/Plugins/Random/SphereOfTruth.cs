﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Types;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class SphereOfTruth : IPlugin
    {
        private readonly Random _generator = new Random();

        private readonly string[] _options =
        {
            "Вперед!",
            "Не сейчас",
            "Не делай этого",
            "Ты шутишь?",
            "Да, но позднее",
            "Думаю, не стоит",
            "Не надейся на это",
            "Ни в коем случае",
            "Это неплохо",
            "Кто знает?",
            "Туманно будущее...",
            "Я не уверен",
            "Я думаю, хорошо",
            "Забудь об этом",
            "Это возможно",
            "Определенно - да ",
            "Быть может",
            "Слишком рано",
            "Да",
            "Конечно, да",
            "Даже не думай",
            "Лучше Вам пока этого не знать"
        };

        public IEnumerable<string> Commands { get; } = new[] {"шар"};

        public async Task Handle(Settings settings, VkMessage message)
        {
            var peer = message.Peer;
            var responce = _options[_generator.Next(_options.Length)];
            await settings.Api.Messages.Send(peerId: peer, message: responce);
        }
    }
}