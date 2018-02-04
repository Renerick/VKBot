using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// <param name="settings">Bot settings</param>
        /// <param name="message">Message object</param>
        Task Handle(Settings settings, VkMessage message);
    }
}