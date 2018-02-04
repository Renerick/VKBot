using VkLibrary.Core;
using VkLibrary.Core.Methods;

namespace VKBot.Core.Common.Services
{
    /// <summary>
    /// This class is a proxy for library Message module
    /// </summary>
    public static class MessageService
    {
        private static Messages _m;

        public static void SetApi(Vkontakte vk)
        {
            _m = vk.Messages;
        }

        public static Messages Perform => _m;
    }
}