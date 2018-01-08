using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Types;

namespace VKBot.Plugins
{
    [VkBotPlugin]
    internal class TestPlugin : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"тест"};

        public async Task Handle(Settings settings, VkMessage message)
        {
            await settings.Api.Messages.Send(peerId: message.Peer, message: message.Text + "!");
        }
    }
}