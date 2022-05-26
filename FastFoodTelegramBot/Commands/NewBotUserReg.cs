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
using FastFoodTelegramBot.Repositories;
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot.Commands
{
    class NewBotUserReg : ICommand
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            Account newacc = new() { ChatId = id, FirstName = firstName, LastName = lastName };
            CommandUserStatus st = new() { ChatId = id, LastCmd = CommandStatus.Start, NextCmd = CommandStatus.Intro };
            CmdUserStatusRepos<CommandUserStatus>.Repos.Add(st);
            //st.LastCmd = CommandStatus.Start;
            //st.NextCmd = CommandStatus.Intro;
            CallBackFunc.@AddItemInDb(newacc, AccListRepos<Account>.Repos);
            await botClient.SendTextMessageAsync(id, $"Your account isn't found in our database.\n" +
                  $"You are added to the database.\n" +
                  $"Your default name is {newacc?.FirstName} {newacc?.LastName}.\n " +
                  $"Your Chat ID is {id}");
        }
    }
}
