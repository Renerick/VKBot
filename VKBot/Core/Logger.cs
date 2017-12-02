using System;
using VkLibrary.Core.Services;

namespace VKBot.Core
{
    public class Logger : ILogger
    {
        public void Log(object o)
        {
            Console.WriteLine($"{DateTime.Now}: {o}");
        }
    }
}