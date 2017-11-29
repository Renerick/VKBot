using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.PluginsManaging
{
    internal class PluginsProvider
    {
        public Dictionary<string, IPlugin> PluginsList { get; } = new Dictionary<string, IPlugin>();

        /// <summary>
        /// Initialize plugins provider
        /// </summary>
        /// <param name="plugins">Collection of plugins to work with</param>
        public PluginsProvider(IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                foreach (var command in plugin.Commands)
                {
                    PluginsList[command] = plugin;
                }
            }
        }

        public Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            //TODO: replace with actual handling
            return PluginsList.Values.First().Handle(settings, tuple);
        }
    }
}