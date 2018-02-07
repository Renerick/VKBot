namespace VKBot.Core.Common
{
    /// <summary>
    ///     This class contains settings of the bot
    /// </summary>
    public class Settings
    {
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