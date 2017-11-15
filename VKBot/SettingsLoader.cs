using System.IO;
using Newtonsoft.Json;
using VKBot.Types;

namespace VKBot
{
    public static class SettingsLoader
    {
        private const string LoginPath = "./login.json";
        private const string ConfigPath = "./config.json";

        public static LoginData LoadLoginData()
        {
            var loginData = ParseFile<LoginData>(LoginPath);
            return loginData;
        }

        public static Settings LoadConfiguration()
        {
            var configuration = ParseFile<Settings>(ConfigPath);
            return configuration;
        }

        private static T ParseFile<T>(string path)
        {
            T parseResult;
            using (var file = new StreamReader(path))
            {
                var fileContent = file.ReadToEnd();

                parseResult = JsonConvert.DeserializeObject<T>(fileContent);
            }
            return parseResult;
        }
    }
}