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
    class OrderCmd : ICommand 
    {
        internal static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
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

            StringBuilder sb = new StringBuilder();
            float totalprice = 0;
            st.LastCmd = CommandStatus.View;
            ShopCartItemRepository<ShopCartItem<Product>> usrshopcart = UserShopCartItemRepos<ShopCartItemRepository<ShopCartItem<Product>>, ShopCartItem<Product>>.GetShopCartItemListById(id);
            //usrshopcart.Repos.Clear();
            sb.AppendLine("Check your registration data");
            sb.AppendLine("============================");
            sb.AppendLine(foundAccount.ToString());
            sb.AppendLine("============================");
            sb.AppendLine("Check selected products");
            sb.AppendLine("============================");

            for (int i = 0; i < usrshopcart.Repos.Count; i++)  //
            {
                sb.AppendLine(usrshopcart.Repos[i].ToString());
                sb.AppendLine("============================");
                totalprice += usrshopcart.Repos[i].PositionPrice();
            }
            
            string tprice = string.Format("{0:f2}", totalprice);
            sb.AppendLine($"Total price: {tprice} BYN");
            sb.AppendLine("============================");
            sb.AppendLine("Is everything right?");
            sb.AppendLine("Choose options (press buttons):");
            await botClient.SendTextMessageAsync(id, sb.ToString() ,
                  replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
        }
    }
}
