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
using FastFoodTelegramBot.Repositories;
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot.Services
{
    class CmdService
    {
        internal static async Task CmdSrv(ITelegramBotClient botClient, Update update, CancellationToken cts)
        {
            long id; string? msg, username, firstName, lastName;
            var newUser = update.Type; 

            switch (update.Type)
            {
                case (UpdateType.Message):
                    id = update.Message.Chat.Id;
                    msg = update.Message.Text;
                    username = update.Message.Chat.Username;
                    firstName = update.Message.Chat.FirstName;
                    lastName = update.Message.Chat.LastName;
                    break;
                case (UpdateType.CallbackQuery):
                    id = update.CallbackQuery.Message.Chat.Id;
                    msg = update.CallbackQuery.Data;
                    username = update.CallbackQuery.From.Username;
                    firstName = update.CallbackQuery.Message.From.FirstName;
                    lastName = update.CallbackQuery.Message.From.LastName;
                    break;
                default: return;
            }

            LogHelper.Info($"Telegram Update Type: {update.Type}\nID:{id}\nMessage:{msg}\nUsername:{username}\n" +
                           $"First Name:{firstName}. Last Name:{lastName}.");

            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }

            CmdItem? tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefKeyCmd==msg.ToLower()) || (p.PrefKeyCmd2== msg.ToLower()));
           
            if (tempItem != null)
            {
                await tempItem.CmdExec(botClient, id, msg, firstName, lastName);
                return;
            }

            if (msg.ToLower().StartsWith("/cart"))
            {
                tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefNextCmd == CommandStatus.AdditemToCart));
                await tempItem.CmdExec(botClient, id, msg, firstName, lastName);
                return;
            }

            if (msg.ToLower().StartsWith("/modcartitem") || msg.ToLower().StartsWith("/delcartitem"))
            {
                tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefNextCmd == CommandStatus.ModifyCartItemInp));
                await tempItem.CmdExec(botClient, id, msg, firstName, lastName);
                return;
            }

            
            tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefNextCmd == st.NextCmd));
            if (tempItem != null)
            {
                await tempItem.CmdExec(botClient, id, msg, firstName, lastName);
                return;
            }

                await botClient.SendTextMessageAsync(id, "Your command is not recognized.\n " +
                  "Choose command again.", replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
        }
    }
}
