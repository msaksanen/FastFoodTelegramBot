using System;
using System.Collections.Generic;
using System.Linq;
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
//using FastFoodTelegramBot.Keyboards;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot.Commands
{
    interface ICommand
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            await botClient.SendTextMessageAsync(id, $"Your command is not recognized.\n " +
                  $"The bot will be restarted.", replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
        }
    }
}
