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
    class ProdCatCmd : ICommand
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            // CmdItem? tempItem = CmdListRepository<CmdItem>.Repos.Find(p => (p.PrefKeyCmd.Equals(msg, StringComparison.OrdinalIgnoreCase)));
            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }

            ProdCategory? tempProdCat=ProdCatRepository.CatList.Find(p => (p.PrefKeyCmd==msg.ToLower())); 
            if (tempProdCat.SubCatList.Count()!=0)
            {
                st.NextCmd = CommandStatus.ProdCat;
                st.NextCmdStr = tempProdCat.Name;
                st.LastCmd = CommandStatus.View;
                await botClient.SendTextMessageAsync(id, $"View our {tempProdCat.Name} catalogue, please.", 
                      replyMarkup: KbdMaker.InKeyboard(tempProdCat.ProdSubCatBtn));
            }
            else
            {
                st.NextCmd = CommandStatus.AdditemToCart;
                st.LastCmd = CommandStatus.View;
                st.NextCmdStr = tempProdCat.Name;
                await SearchListMaker<Product>.Command(botClient, id, msg, firstName, lastName, tempProdCat.PrefKeyCmd);
                SearchProdListRepository<Product> usrsprod = UserSearchProdListRepos<SearchProdListRepository<Product>, Product>.GetSearchProdListById(id);

                if (usrsprod.Repos.Count==0) //(st.SearchIndexLength ==0)
                {
                    st.NextCmd =CommandStatus.View;
                    st.LastCmd = CommandStatus.View;
                    await botClient.SendTextMessageAsync(id, $"Products of this category are not found. Review our catalogue again.",
                          replyMarkup: KbdMaker.InKeyboard(CmdItemInit.MainProdCatBtn));
                    return;

                }
                else await SearchListPrint<Product>.Command(botClient, id, msg, firstName, lastName);
            }
        }
    }
}
