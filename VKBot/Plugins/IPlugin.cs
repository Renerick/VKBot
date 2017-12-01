using System.Collections.Generic;
using System.Threading.Tasks;
using VKBot.Types;

namespace VKBot.Plugins
{
    /// <summary>
    ///     This interface for all plugins
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// List of commands
        /// </summary>
        IEnumerable<string> Commands { get; }

        /// <summary>
        /// Method which handle new message
        /// </summary>
        /// <param name="settings">Bot settings</param>
        /// <param name="message">Message object</param>
        Task Handle(Settings settings, VkMessage message);
    }
}