using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.PluginsManaging
{
    public class Plugins
    {
        public IPlugin[] PluginsList { get; }

        public Plugins(IPlugin[] pluginsList)
        {
            PluginsList = pluginsList ?? throw new ArgumentNullException(nameof(pluginsList));
        }

        public Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            //TODO: replace with actual handling
            return PluginsList.First().Handle(settings, tuple);
        }
    }
}