using System.Collections.Generic;
using Newtonsoft.Json;
using VkLibrary.Core.Types.Account;

namespace VKBot.Types
{
    /// <summary>
    ///     This class contains settings of the bot
    /// </summary>
    public class Settings
    {
        /// <summary>
        ///     Information about account used by bot
        /// </summary>
        [JsonIgnore]
        public UserSettings BotAccount { get; set; } = null;

        /// <summary>
        ///     Users rights storage
        /// </summary>
        public Dictionary<ulong, sbyte> UsersLevels { get; internal set; }

        /// <summary>
        ///     Default user rights level
        /// </summary>
        public sbyte DefaultLevel { get; internal set; }
    }
}