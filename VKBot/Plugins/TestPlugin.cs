﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VkLibrary.Core.LongPolling;
using VKBot.Types;

namespace VKBot.Plugins
{
    class TestPlugin : IPlugin
    {
        public string[] Commands { get; } = {"тест"};

        public Task Handle(Settings settings, Tuple<int, MessageFlags, JArray> tuple)
        {
            var peer = (int) tuple.Item3[3];
            var message = (string) tuple.Item3[6];

            return settings.Api.Messages.Send(peerId: peer, message: message + "!");
        }
    }
}