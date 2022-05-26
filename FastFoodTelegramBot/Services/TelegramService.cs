using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using FastFoodTelegramBot.Services;
using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Utilities;
using FastFoodTelegramBot.Commands;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot
{
    class TelegramService
    {
       // public static string TGToken { get; set; }= "5202235781:AAGx13pyeOj8A5beMAme48WrCXmsBRqOcZc";

        public static void TelegramStart()
        {  
            var botClient = new TelegramBotClient(CommandNames.TGToken);
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            botClient.StartReceiving(
                HandleUpdatesAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            Console.WriteLine("The bot started " + botClient.GetMeAsync().Result.FirstName);
        }
        internal static async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cts)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
           // LogHelper.Log(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update?.Message?.Text == null && update?.CallbackQuery == null )
            {
                LogHelper.Info("Telegram message is null");
                return;
            }
               
            await CmdService.CmdSrv(botClient, update, cts);
        }

        internal static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };
            //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
            LogHelper.Error(exception);
            Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
    }
}
