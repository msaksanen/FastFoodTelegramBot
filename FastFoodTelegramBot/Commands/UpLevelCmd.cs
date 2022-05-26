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
    class UpLevelCmd : ICommand
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            CmdItem? tempItem;

            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }

            if (st.LastCmd == CommandStatus.ProdCat)
            {
                tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefNextCmdStr== st.NextCmdStr));
                if (tempItem != null)
                {
                    await botClient.SendTextMessageAsync(id, "OK. Return to upper menu level",
                          replyMarkup: KbdMaker.InKeyboard(tempItem.BtnCmd));
                    return;
                }
            }

            tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefNextCmd ==st.LastCmd));
            if (tempItem!=null)
            { 
               await botClient.SendTextMessageAsync(id, "OK. Return to upper menu level",
                     replyMarkup: KbdMaker.InKeyboard(tempItem.BtnCmd));
            }
            else
            {
              await botClient.SendTextMessageAsync(id, $"Your command is not recognized." +
                    $"Return to the first menu level.", replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
            }
        }
    }
}
