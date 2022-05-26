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
    class ModifyCartItemInp2Cmd<T> : ICommand where T : Product
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

            bool isUint = uint.TryParse(msg, out uint qty);
            ShopCartItem<Product>? tempCartItem = default;
          
            tempCartItem = usrshopcart.Repos.Find(p => p.CartItemId.ToString() == st.LastCartItemdId);

            if (!isUint)
            {
                st.NextCmd = CommandStatus.ModifyCartItemInp2;
                st.LastCmd = CommandStatus.ViewCart;           
                await botClient.SendTextMessageAsync(id, "You have input incorrect number of portions.\n" + tempCartItem.ToString() +
                      $"\nInput correct number to conitune or choose other options (press buttons)",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.ModifyCartBtnCmd, tempCartItem.CartItemId.ToString()));
                return;
            }
            else
            {
                st.LastCmd = CommandStatus.View;
                st.NextCmd = CommandStatus.ViewCart;
                if (qty==0)
                {
                    await DelCartItemCmd<T>.Command(botClient, id, msg, firstName, lastName);
                    return;
                }
                tempCartItem.Quantity = qty;
                CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                await botClient.SendTextMessageAsync(id, "Product quantity has been modified in the cart.\n" +
                      tempCartItem.ToString(), replyMarkup: KbdMaker.MainKeyboard);

            }
        }
    }
}
