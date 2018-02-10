using System.Collections.Generic;
using System.Threading.Tasks;
using VkLibrary.Core;
using VkLibrary.Core.Services;
using VKBot.Core.Common;

namespace VKBot.Plugins
{
    /// <summary>
    ///     This interface for all plugins
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        ///     List of commands
        /// </summary>
        IEnumerable<string> Commands { get; }

        /// <summary>
        ///     Handle new message
        /// </summary>
        /// <param name="message">Message object</param>
        /// <param name="api"></param>
        /// <param name="logger"></param>
        Task Handle(VkMessage message, Vkontakte api, ILogger logger);
    }
}