using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    public class SphereOfTruth : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"шар"};

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

        private readonly Random _generator = new Random();

        public async Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = _options[_generator.Next(_options.Length)];
            await settings.Api.Messages.Send(peerId: peer, message: message);
        }
    }
}