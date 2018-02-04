using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VKBot.Plugins;

namespace VKBot.Core.Common.Services
{
    internal class PluginsService
    {
        /// <summary>
        ///     Initialize plugins service
        /// </summary>
        public PluginsService()
        {
            PluginsDict = new Dictionary<string, IPlugin>();

            _initPlugins();
        }

        private void _initPlugins()
        {
            var plugins = new ReadOnlyCollectionBuilder<IPlugin>();
            var classes = AppDomain.CurrentDomain.GetAssemblies()
                                   .SelectMany(x => x.GetTypes())
                                   .Where(x => Attribute.IsDefined(x, typeof(VkBotPluginAttribute)));

            foreach (var plugin in classes)
                plugins.Add((IPlugin) Activator.CreateInstance(plugin));

            foreach (var plugin in plugins)
            foreach (var command in plugin.Commands)
                PluginsDict[command.ToLowerInvariant()] = plugin;
        }

        private Dictionary<string, IPlugin> PluginsDict { get; }

        /// <summary>
        ///     Handle new message
        /// </summary>
        /// <param name="message">New message to handle</param>
        public void HandleMessage(VkMessage message)
        {
            var body = message.Body;
            var spaceIndex = body.IndexOf(" ", StringComparison.Ordinal);

            var command = body.Substring(0, spaceIndex > 0 ? spaceIndex : body.Length);

            if (!PluginsDict.TryGetValue(command, out var plugin)) return;
            try
            {
                plugin.Handle(message);
            }
            catch (Exception e)
            {
                LoggerService.Logger.Log(e);
            }
        }
    }
}