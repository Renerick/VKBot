using VkLibrary.Core.Services;

namespace VKBot.Core.Services
{
    public static class LoggerService
    {
        public static ILogger Logger { get; set; }

        public static void Log(object obj)
        {
            Logger.Log(obj);
        }
    }
}