using VkLibrary.Core;
using VkLibrary.Core.Services;

namespace VKBot.Core.Services
{
    public class ServiceContext
    {
        public LoggerService Logger { get; }
        public Vkontakte ApiService { get; }

        public ServiceContext(ILogger logger, Vkontakte api)
        {
            Logger = new LoggerService(logger);
            ApiService = api;
        }
    }
}