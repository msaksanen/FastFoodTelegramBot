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
    class ProdSubCatCmd : ICommand
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }

            ProdCategory tempCat=default;
            ProdSubCategory tempSubCat=default;
            //Searches subcategory by keyword in all categories and their subcategories.
            foreach (ProdCategory prodCat in ProdCatRepository.CatList)
            {
                foreach (ProdSubCategory subCat in prodCat.SubCatList)
                {
                    if (subCat.PrefKeyCmd.Equals(msg.ToLower(), StringComparison.OrdinalIgnoreCase))
                    {
                        tempCat = prodCat;
                        tempSubCat = subCat;
                    }
                }
            }
            if (tempCat!=default && tempSubCat!=default)
            {
                st.NextCmd = CommandStatus.AdditemToCart;
                st.LastCmd = CommandStatus.ProdCat;
                st.NextCmdStr = tempSubCat.Name;
                await SearchListMaker<Product>.Command(botClient, id, msg, firstName, lastName, tempSubCat.PrefKeyCmd);
                SearchProdListRepository<Product> usrsprod = UserSearchProdListRepos<SearchProdListRepository<Product>, Product>.GetSearchProdListById(id);

                if (usrsprod.Repos.Count == 0) //(st.SearchIndexLength ==0)
                {
                    st.NextCmd = CommandStatus.View;
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
