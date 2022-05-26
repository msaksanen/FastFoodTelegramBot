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
    class ViewCartCmd<T> : ICommand where T : Product
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

            StringBuilder sb = new StringBuilder();
            float totalprice = 0;
            st.LastCmd = CommandStatus.View;

            //if (msg.Equals("View Cart", StringComparison.OrdinalIgnoreCase))
            
                for (int i = 0; i < usrshopcart.Repos.Count; i++) //(int i=0; i < st.ShopCartItemQty; i++)
            {
                    sb.AppendLine(usrshopcart.Repos[i].ToString());
                    sb.AppendLine("============================");
                    totalprice += usrshopcart.Repos[i].PositionPrice();
                }

            string tprice = string.Format("{0:f2}", totalprice);
            await botClient.SendTextMessageAsync(id, "Your cart. Contents:\n" + sb +
                  $"Total price: {tprice} BYN",
                  replyMarkup: KbdMaker.MainKeyboard);
                return;
         
           
        }
    }
}
