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
    class ItemQtyToCartCmd<T> : ICommand where T:Product
    {
        internal static async Task Command(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName)
        {
            bool isUint = uint.TryParse(msg, out uint qty);
            Product? tempProd = default;

            Account foundAccount = CallBackFunc.@GetByID(AccListRepos<Account>.Repos, id, out bool isFound);
            CommandUserStatus st = CmdUserStatusRepos<CommandUserStatus>.GetStatusById(id);
            ShopCartItemRepository<ShopCartItem<Product>> usrshopcart = UserShopCartItemRepos<ShopCartItemRepository<ShopCartItem<Product>>, ShopCartItem<Product>>.GetShopCartItemListById(id);
            SearchProdListRepository<Product> usrsprod = UserSearchProdListRepos<SearchProdListRepository<Product>, Product>.GetSearchProdListById(id);

            if (!isFound)
            {
                await NewBotUserReg.Command(botClient, id, msg, firstName, lastName);
                await botClient.SendTextMessageAsync(id, "Would you like to make order in our shop? Choose options.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.IntroBtnCmd));
                return;
            }

            if (!isUint || qty==0)
            {
                st.NextCmd = CommandStatus.ItemQtyToCart;
                st.LastCmd = CommandStatus.ProdCat;
                tempProd = usrsprod.Repos.Find(p => p.Id.ToString() == st.LastProdId);
                await botClient.SendTextMessageAsync(id, "You have input incorrect number of portions.\n" + tempProd.ToString()+
                      $"\nInput correct number to conitune or choose other options (press buttons)", 
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.ItemToCartBtnCmd, tempProd.Id.ToString()));
                return;
            }
            else
            {
                st.LastCmd = CommandStatus.View;
                st.NextCmd = CommandStatus.AdditemToCart;
                tempProd = usrsprod.Repos.Find(p => p.Id.ToString() == st.LastProdId);
                ShopCartItem<Product> cartItem = new() { StringProdId = st.LastProdId, Quantity = qty, ChatID = id, ProducName=tempProd.Name };
                //st.ShopCartItemQty++;
                usrshopcart.Repos.Add(cartItem);
                CallBackFunc.@UpdateAccount(foundAccount, AccListRepos<Account>.Repos);
                await botClient.SendTextMessageAsync(id, "You product has been added to cart.\n"+
                      cartItem.ToString(), replyMarkup: KbdMaker.MainKeyboard);
                
            }
        }
    }
}
