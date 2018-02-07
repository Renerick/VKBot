using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VKBot.Core.Common;
using VKBot.Core.Services;
using VKBot.Plugins;

namespace VKBot.Core
{
    internal class PluginsService
    {
        private readonly ServiceContext _services;
        
        /// <summary>
        ///     Initialize plugins service
        /// </summary>
        public PluginsService(ServiceContext services)
        {
            PluginsDict = new Dictionary<string, IPlugin>();
            _services = services;
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
                plugin.Handle(message, _services);
            }
            catch (Exception e)
            {
                _services.Logger.Log(e);
            }
        }
    }
}