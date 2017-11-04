using System;
using System.IO;
using Newtonsoft.Json;
using VkNet;
using VkNet.Exception;
using VKBot.Types;

namespace VKBot
{
    internal static class Program
    {
        // instance of api
        private static readonly VkApi Vk = new VkApi();

        // settings storage
        private static Settings _settings;

        private static void Main()
        {
            if (Configure()) return;

            // Message about successful login
            // TODO: this is temporary solution, need to remove it and implement nice logging

            Console.WriteLine("Successfuly logged in");

            if (Vk.UserId.HasValue)
            {
                _settings.BotAccount = Vk.Users.Get(Vk.UserId.Value);
                Console.WriteLine(
                    $"Account: {_settings.BotAccount.Id}, {_settings.BotAccount.FirstName} {_settings.BotAccount.LastName}");
            }

            Console.WriteLine("Configuration finished");
        }

        private static bool Configure()
        {
            try
            {
                var apiAuthParams = SettingsLoader.LoadApiAuthParams();
                Vk.Authorize(apiAuthParams);
                _settings = SettingsLoader.LoadConfiguration();
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Configuration parse failure, {e.Message}");
                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine($"File reading failure, {e.Message}");
                return true;
            }
            catch (VkApiAuthorizationException e)
            {
                Console.WriteLine($"Authentification error, {e.Message}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception, {e}");
                return true;
            }
            return false;
        }
    }
}