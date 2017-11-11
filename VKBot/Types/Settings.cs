using System.Collections.Generic;

namespace VKBot.Types
{
    /// <summary>
    ///     This class contains settings of the bot
    /// </summary>
    public class Settings
    {
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