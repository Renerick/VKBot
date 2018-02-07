using VkLibrary.Core.Services;

namespace VKBot.Core.Services
{
    public class LoggerService
    {
        public ILogger Logger { get; set; }

        public void Log(object obj)
        {
            Logger.Log(obj);
        }

        public LoggerService(ILogger logger)
        {
            Logger = logger;
        }
    }
}