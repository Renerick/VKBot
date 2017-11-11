using System;
using System.IO;
using Newtonsoft.Json;

namespace VKBot
{
    internal static class Program
    {
        private static void Main()
        {
            VkBot bot;
            try
            {
                var loginData = SettingsLoader.LoadApiAuthParams();
                var settings = SettingsLoader.LoadConfiguration();
                bot = new VkBot(loginData, settings, Console.WriteLine);
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Configuration parse failure, {e.Message}");
                return;
            }
            catch (IOException e)
            {
                Console.WriteLine($"File reading failure, {e.Message}");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception, {e}");
                return;
            }
            
            bot.StartBot();
        }
    }
}