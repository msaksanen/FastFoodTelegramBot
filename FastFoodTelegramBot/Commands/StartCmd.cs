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
    class StartCmd:ICommand 
    {
        public static async Task Command (ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
                Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
                CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);

            if (isFound)
                {
                    st.LastCmd= CommandStatus.Start;
                    st.NextCmd = CommandStatus.Intro;
                    ShopCartItemRepository<ShopCartItem<Product>> usrshopcart = UserShopCartItemRepos <ShopCartItemRepository<ShopCartItem<Product>>, ShopCartItem<Product>>. GetShopCartItemListById(id);
                    usrshopcart.Repos.Clear();
                    st.SearchListIndex = 0;
                    //st.SearchIndexLength = 0;
                    //st.ShopCartItemQty = 0;
                    st.LastProdId = string.Empty;
                    CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                    await botClient.SendTextMessageAsync(id, $"Hello, new user. Glad to see you again. " +
                          $"Your name is {foundAccount?.FirstName} {foundAccount?.LastName}.Your Chat ID is {id}.\n" +
                          $"Would you like to make order in our shop? Choose options.", replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                }
                else
                {
                    await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);  
                    await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.", 
                          replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                }
        }
    }
}
