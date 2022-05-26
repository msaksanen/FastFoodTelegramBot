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
    class RestartBotCmd : ICommand
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

            st.NextCmd = CommandStatus.Start;
            st.LastCmd = CommandStatus.Intro;

            usrshopcart.Repos.Clear();
            st.SearchListIndex = 0;
            //st.SearchIndexLength = 0;
            //st.ShopCartItemQty = 0;
            st.LastProdId = string.Empty;
            await botClient.SendTextMessageAsync(id, $"The bot has been restarted." +
                  $"You cart is cleared.", replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
        }
    }
}
