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
    class ModifyCartCmd<T> : ICommand where T : Product
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

            ShopCartItem<Product>? tempCartItem;
            float totalprice = 0;
            st.LastCmd = CommandStatus.View;
            st.NextCmd = CommandStatus.ModifyCartItemInp;
           
            for (int i = 0; i < usrshopcart.Repos.Count; i++) //(int i = 0; i < st.ShopCartItemQty; i++)
            {
                tempCartItem = usrshopcart.Repos[i];    
                //var  tempProd = ProductListRepository<T>.Repos.Find(p => p.Id.ToString() ==tempCartItem.StringId);
                await botClient.SendTextMessageAsync(id, "Your cart. Contents:\n" + tempCartItem.ToString(),
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.ModifyCartBtnCmd, tempCartItem.CartItemId.ToString()));

                totalprice += usrshopcart.Repos[i].PositionPrice();
            }

            string tprice = string.Format("{0:f2}", totalprice);
            await botClient.SendTextMessageAsync(id, $"Your cart. Total price: {tprice} BYN",
                  replyMarkup: KbdMaker.MainKeyboard);
            return;
        }
    }
}
