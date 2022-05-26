﻿using System;
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
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot.Commands
{
    class FirstNameInpCmd:ICommand
    {
        internal static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            Account foundAccount;
            bool isFound;

            foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out isFound);
            CommandUserStatus st = Repositories.CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, $"Input additional info, please.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.RegBtnCmd));
                return;
            }

            if (st.NextCmdStr == CommandNames.InpNameF)
            {
                string? firstname = msg;
                foundAccount.FirstName = firstname;
                CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                st.NextCmdStr = default;
                st.LastCmd = CommandStatus.Intro; //Return to upper level.
                st.NextCmd = CommandStatus.Reg;
                await botClient.SendTextMessageAsync(id, $"OK.Your first name is {foundAccount?.FirstName}. ", replyMarkup: KbdMaker.InKeyboard(CommandNames.RegBtnCmd));
              
            }
        }
    }
}
