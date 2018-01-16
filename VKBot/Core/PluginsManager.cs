using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VKBot.Plugins;
using VKBot.Types;

namespace VKBot.Core
{
    internal class PluginsManager
    {
        /// <summary>
        ///     Initialize plugins provider
        /// </summary>
        public PluginsManager()
        {
            var plugins = new ReadOnlyCollectionBuilder<IPlugin>();

            var classes = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(x => x.GetTypes())
                                   .Where(x => Attribute.IsDefined(x, typeof(VkBotPluginAttribute)));

            foreach (var plugin in classes)
                plugins.Add((IPlugin) Activator.CreateInstance(plugin));

            foreach (var plugin in plugins)
            foreach (var command in plugin.Commands)
                PluginsDict[command] = plugin;
        }

        private Dictionary<string, IPlugin> PluginsDict { get; } = new Dictionary<string, IPlugin>();

        /// <summary>
        ///     Handle new message
        /// </summary>
        /// <param name="settings">Bot settings</param>
        /// <param name="message">New message to handle</param>
        public void Handle(Settings settings, VkMessage message)
        {
            var messageTokens = message.Text.Split();

            if (messageTokens.Length < 2) return;
            var command = messageTokens[1];

            if (PluginsDict.TryGetValue(command, out var plugin))
                plugin.Handle(settings, message);
        }
    }
}