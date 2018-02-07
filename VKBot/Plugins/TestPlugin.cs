using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;
using VKBot.Core.Services;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    internal class TestPlugin : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"тест"};

        public async Task Handle(VkMessage message, ServiceContext services)
        {
            await services.ApiService.Messages.Send(peerId: message.Peer, message: message.Text + "!");
        }
    }
}