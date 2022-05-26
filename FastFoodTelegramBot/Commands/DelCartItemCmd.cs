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
    class DelCartItemCmd<T> : ICommand where T : Product
    {
        internal static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            ShopCartItemRepository<ShopCartItem<Product>> usrshopcart = UserShopCartItemRepos<ShopCartItemRepository<ShopCartItem<Product>>, ShopCartItem<Product>>.GetShopCartItemListById(id);

            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }
       
            ShopCartItem<Product>? tempCartItem = default;
            tempCartItem = usrshopcart.Repos.Find(p => p.CartItemId.ToString() == st.LastCartItemdId);

            if (tempCartItem == default)
            {
                st.NextCmd = CommandStatus.ViewCart;
                st.LastCmd = CommandStatus.View;
                await botClient.SendTextMessageAsync(id, $"The item wasn't found. View your cart again, please.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));
                return;
            }

            if (st.LastCartItemdId != string.Empty)
            {
                st.NextCmd = CommandStatus.ModifyCartItem;
                st.LastCmd = CommandStatus.ViewCart;
                st.LastCartItemdId = string.Empty;
                string tempstr = tempCartItem.ToString();
                //st.ShopCartItemQty--;
                usrshopcart.Repos.Remove(tempCartItem);
                CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                await botClient.SendTextMessageAsync(id,
                          tempstr + "\nItem removed from you cart.",
                          replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));
                
            }
        }
    }
}
