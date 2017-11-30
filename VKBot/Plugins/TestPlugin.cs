using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    class TestPlugin : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"тест"};

        public async Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            await settings.Api.Messages.Send(peerId: peer, message: message + "!");
        }
    }
}