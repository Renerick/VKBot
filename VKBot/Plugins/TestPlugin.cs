using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;
using VKBot.Core.Common.Services;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    internal class TestPlugin : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"тест"};

        public async Task Handle(VkMessage message)
        {
            await MessageService.Perform.Send(peerId: message.Peer, message: message.Text + "!");
        }
    }
}