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
    class EmailInpCmd
    {
        internal static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            Account foundAccount;
            bool isFound;

            foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, $"Input additional info, please.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.RegBtnCmd));
                return;
            }

            string? email = msg;
            if (RegExUtil.IsValidEmail(email))
            {
                foundAccount.Email = email;
                CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                st.NextCmdStr = default;
                st.LastCmd = CommandStatus.Intro; //Return to upper level.
                st.NextCmd = CommandStatus.Reg;
                await botClient.SendTextMessageAsync(id, $"OK.Your e-mail {foundAccount?.Email} is valid. ", 
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.RegBtnCmd));
            }
            else
            {
                st.NextCmd = CommandStatus.EmailInp;
                st.LastCmd = CommandStatus.Reg;
                st.NextCmdStr = CommandNames.InpMail;
                await botClient.SendTextMessageAsync(id, $"Your email has incorrect format. Input correct data.", 
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));
            }
        }
    }
}
