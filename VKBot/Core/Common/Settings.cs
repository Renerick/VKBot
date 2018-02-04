using VkLibrary.Core;

namespace VKBot.Core.Common
{
    /// <summary>
    ///     This class contains settings of the bot
    /// </summary>
    public class Settings
    {
        /// <summary>
        ///     Vk API reference
        /// </summary>
        public Vkontakte Api { get; internal set; }

        /// <summary>
        ///     Bot account ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Array containing a set of the bot command prefixes
        /// </summary>
        public string[] Prefixes { get; set; }
    }
}