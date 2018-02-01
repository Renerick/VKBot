﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VKBot.Plugins;
using VKBot.Types;

namespace VKBot.Core
{
    internal class PluginsService
    {
        /// <summary>
        ///     Initialize plugins provider
        /// </summary>
        public PluginsService()
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

        private Dictionary<string, IPlugin> PluginsDict { get; } = new Dictionary<string, IPlugin>();

        /// <summary>
        ///     Handle new message
        /// </summary>
        /// <param name="settings">Bot settings</param>
        /// <param name="message">New message to handle</param>
        public void Handle(Settings settings, VkMessage message)
        {
            var messageTokens = message.Command.Split();

            var command = messageTokens[0].ToLowerInvariant();
            Console.WriteLine(command);

            if (!PluginsDict.TryGetValue(command, out var plugin)) return;
            try
            {
                plugin.Handle(settings, message);
            }
            catch (Exception e)
            {
                // TODO: refactor it
                Console.WriteLine(e);
            }
        }
    }
}