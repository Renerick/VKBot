using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Core.Common;
using VKBot.Core.Services;

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
        /// <param name="services">Services of the bot (logger, api, etc.)</param>
        Task Handle(VkMessage message, ServiceContext services);
    }
}