using System;
using System.IO;
using Newtonsoft.Json;
using VkNet;
using VkNet.Enums.Filters;
using VKBot.Types;
using Settings = VKBot.Types.Settings;

namespace VKBot
{
    public static class SettingsLoader
    {
        private const string LoginPath = "./login.json";
        private const string ConfigPath = "./config.json";

        public static ApiAuthParams LoadApiAuthParams()
        {
            var apiAuthParams = ParseFile<ApiAuthParams>(LoginPath);

            apiAuthParams.Settings = VkNet.Enums.Filters.Settings.All;
            apiAuthParams.TwoFactorAuthorization = TwoFactorAuthorization;

            return apiAuthParams;


            string TwoFactorAuthorization()
            {
                Console.WriteLine("Two factor authentification requested. Enter confirmation code:");
                return Console.ReadLine();
            }
        }

        public static Settings LoadConfiguration()
        {
            try
            {
                var configuration = ParseFile<Settings>(ConfigPath);

                return configuration;
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }
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