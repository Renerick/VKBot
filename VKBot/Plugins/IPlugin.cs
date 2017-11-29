using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Plugins
{
    /// <summary>
    ///     This interface for all plugins
    /// </summary>
    public interface IPlugin
    {
        IEnumerable<string> Commands { get; }

        Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple);
    }
}