using System.Collections.Generic;
using Newtonsoft.Json;
using VkNet;
using VkNet.Categories;
using VkNet.Model;

namespace VKBot.Types
{
    /// <summary>
    /// This class contains settings of the bot
    /// </summary>
    public class Settings
    {
        [JsonIgnore]
        public User BotAccount { get; set; }
        
        /// <summary>
        /// Users rights storage
        /// </summary>
        public Dictionary<ulong, sbyte> UsersLevels { get; internal set; }
        /// <summary>
        /// Defalult user rights level
        /// </summary>
        public uint DefaultLevel { get; internal set; }
    }
}