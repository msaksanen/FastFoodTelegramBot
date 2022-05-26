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
using FastFoodTelegramBot.Repositories;
using FastFoodTelegramBot.Utilities;
//using FastFoodTelegramBot.Keyboards;
using static FastFoodTelegramBot.CommandNames;
using FastFoodTelegramBot.Commands;
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot.Utilities
{
    class SearchListMaker<T> : ICommand  where T:Product
    {
        public static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName, string searchKey)
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

            SearchProdListRepository<Product> usrsprod = UserSearchProdListRepos<SearchProdListRepository<Product>, Product>.GetSearchProdListById(id);
            //usrsprod.Repos.Clear();
            if (usrsprod.Repos.Count>0)
            {
                st.SearchListIndex = usrsprod.Repos.Count;
            }
            List<Product>? searchList = ProductListRepository<Product>.Repos.FindAll(p => p.CategoryCmd == searchKey);

            //st.SearchIndexLength = searchList.Count;
            //st.SearchListIndex = 0;
            usrsprod.Repos.AddRange(searchList);
            CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);

        }
       
    }
}
