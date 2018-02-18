using System;

namespace VKBot.Plugins
{
    [AttributeUsage(AttributeTargets.Class)]
    public class VkBotPluginAttribute : Attribute
    {
        public string Name { get; }
        public VkBotPluginAttribute(string name)
        {
            Name = name;
        }
    }
}