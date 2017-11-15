using System;
using System.IO;
using Newtonsoft.Json;

namespace VKBot
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var loginData = SettingsLoader.LoadLoginData();
                var settings = SettingsLoader.LoadConfiguration();
                var bot = new VkBot(loginData, settings, new Logger());
                bot.StartBot();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Configuration parse failure, {e.Message}");
            }
            catch (IOException e)
            {
                Console.WriteLine($"File reading failure, {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception, {e}");
            }
        }
    }
}