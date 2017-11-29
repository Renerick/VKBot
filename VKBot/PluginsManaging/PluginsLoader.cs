using VKBot.Plugins;
using VKBot.Types;

namespace VKBot.PluginsManaging
{
    internal class PluginsLoader
    {
        public static PluginsProvider InitPlugins()
        {
            // TODO: replace with actual loader
            return new PluginsProvider(new IPlugin[]{new TestPlugin()});
        }
    }
}