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
    class SearchListPrint<T>:ICommand where T:Product
    { 
        internal static async Task Command (ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
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

            Product? tempProd;
            SearchProdListRepository<Product> usrsprod = UserSearchProdListRepos<SearchProdListRepository<Product>, Product>.GetSearchProdListById(id);

            //await botClient.SendTextMessageAsync(id, $"View our items, please.", replyMarkup: KbdMaker.InKeyboard(CommandNames.SushiMainBtnCmd));
            while (st.SearchListIndex < usrsprod.Repos.Count)  //(st.SearchListIndex < st.SearchIndexLength)
            {
                tempProd = usrsprod.Repos[st.SearchListIndex];

                st.SearchListIndex++;

                if (tempProd.GetType().GetProperty("ShowPicInCat").GetValue(tempProd).ToString() == "0")
                {
                    await botClient.SendTextMessageAsync(id, $"{tempProd.ToString()}",
                          replyMarkup: KbdMaker.InKeyboard(CommandNames.ItemToCartBtnCmd, tempProd.Id.ToString()));
                }
                else
                {
                    await botClient.SendPhotoAsync(
                          chatId: id,
                          photo: tempProd.PictureURL,
                          caption: $"{tempProd.ToString()}",
                          replyMarkup: KbdMaker.InKeyboard(CommandNames.ItemToCartBtnCmd, tempProd.Id.ToString()));
                }

            }
            if (st.SearchListIndex == st.SearchIndexLength)
            {
                st.LastCmd = CommandStatus.SearchList;
                st.NextCmd = CommandStatus.AdditemToCart;
                st.SearchListIndex = 0;
            }
        }
    }
}
