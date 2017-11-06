namespace VKBot.Types
{
    public class LoginData
    {
        public string AppId { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public string AccessToken { get; set; }
        public int UserId { get; set; }

        public string ApiVersion { get; set; }
    }
}