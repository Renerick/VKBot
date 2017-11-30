using System;
using System.Linq;
using System.Runtime.CompilerServices;
using VKBot.Plugins;

namespace VKBot.PluginsManaging
{
    internal static class PluginsLoader
    {
        /// <summary>
        /// Load and instantiate all classes marked as VkBotPlugin
        /// </summary>
        /// <returns>Read-only collection of plugins</returns>
        public static PluginsProvider InitPlugins()
        {
            var plugins = new ReadOnlyCollectionBuilder<IPlugin>();

            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => Attribute.IsDefined(x, typeof(VkBotPluginAttribute)));
            
            foreach (var plugin in classes)
            {
                plugins.Add((IPlugin) Activator.CreateInstance(plugin));
            }

            return new PluginsProvider(plugins.ToReadOnlyCollection());
        }
    }
}