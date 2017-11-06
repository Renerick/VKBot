using System;
using System.IO;
using Newtonsoft.Json;
using VkLibrary.Core;
using VkLibrary.Core.Auth;
using VKBot.Types;

namespace VKBot
{
    internal static class Program
    {
        // instance of api
        private static Vkontakte _vk;

        // settings storage
        private static Settings _settings;

        private static void Main()
        {
            if (Initialize()) return;

            var longPollParams = _vk.Messages.GetLongPollServer().Result;
            var longPollClient = _vk.StartLongPollClient(longPollParams.Server, longPollParams.Key, longPollParams.Ts)
                .Result;

            longPollClient.AddMessageEvent += async (sender, tuple) =>
            {
                var message = (await _vk.Messages.GetById(new[] {(int?) tuple.Item1})).Items[0];
                if (message.Body.StartsWith("!"))
                    await _vk.Messages.Send(message.UserId, message: "ало ало");
            };
            Console.ReadLine();
        }

        private static bool Initialize()
        {
            try
            {
                var loginData = SettingsLoader.LoadApiAuthParams();
                _settings = SettingsLoader.LoadConfiguration();
                _vk = new Vkontakte(loginData.AppId, JsonParsingType.UseStream);

                if (loginData.AccessToken != null)
                {
                    var accessToken = AccessToken.FromString(loginData.AccessToken, loginData.UserId);
                    _vk.AccessToken = accessToken;

                    _settings.BotAccount = _vk.Account.GetProfileInfo().Result;
                }
                else
                {
                    throw new NotImplementedException("There is no auth through login and password now");
                }
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
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception, {e}");
                return true;
            }
            // Message about successful login
            // TODO: this is temporary solution, need to remove it and implement nice logging

            Console.WriteLine("Successfuly logged in");

            //Console.WriteLine(
            //    $"Account: {_settings.BotAccount.FirstName} {_settings.BotAccount.LastName}");


            Console.WriteLine("Configuration finished");
            return false;
        }
    }
}