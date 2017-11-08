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
                bot = new VkBot(loginData, settings);
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

            // Message about successful login
            // TODO: this is temporary solution, need to remove it and implement nice logging
            Console.WriteLine("Successfuly logged in");
            
            bot.StartBot();
        }
    }
}