using System.Collections.Generic;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Services;
using VKBot.Core.Common;

namespace VKBot.Plugins
{
    [VkBotPlugin("Test plugin")]
    internal class TestPlugin : IPlugin
    {
        public IEnumerable<string> Commands { get; } = new[] {"тест"};

        public async Task Handle(VkMessage message, Vkontakte api, ILogger logger)
        {
            await api.Messages.Send(peerId: message.Peer, message: message.Text + "!");
        }
    }
}