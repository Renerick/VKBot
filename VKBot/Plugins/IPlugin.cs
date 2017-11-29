using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.LongPolling;

namespace VKBot.Types
{
    /// <summary>
    ///     This interface for all plugins
    /// </summary>
    public interface IPlugin
    {
        string[] Commands { get; }

        Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple);
    }
}