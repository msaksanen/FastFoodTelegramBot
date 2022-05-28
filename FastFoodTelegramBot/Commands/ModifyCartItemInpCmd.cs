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
    class ModifyCartItemInpCmd<T> : ICommand where T : Product
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
            string cartItemdId = string.Empty;
          
            if (msg.StartsWith("/modcartitem") || msg.StartsWith("/delcartitem"))
            {
                cartItemdId = msg.Substring(msg.IndexOf('*') + 1, msg.Length - msg.IndexOf('*') - 1);
                tempCartItem = usrshopcart.Repos.Find(p => p.CartItemId.ToString() == cartItemdId);
            }

            if (tempCartItem == default)
            {
                st.NextCmd = CommandStatus.ViewCart;
                st.LastCmd = CommandStatus.View;
                await botClient.SendTextMessageAsync(id, $"The item wasn't found. View your cart again, please.",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));
                return;
            }
            if (cartItemdId != string.Empty)
            {
                st.LastCartItemdId = cartItemdId;
                T? tempProd = ProductListRepository<T>.Repos.Find(p => p.Id.ToString() == tempCartItem.StringProdId);

                if (msg.StartsWith("/modcartitem"))
                {
                    st.NextCmd = CommandStatus.ModifyCartItemInp2;
                    st.LastCmd = CommandStatus.ModifyCartItem;

                    switch (tempProd)
                    {
                        case Sushi tempSushi:
                            await botClient.SendPhotoAsync(
                                chatId: id,
                                photo: tempSushi.PictureURL,
                                caption: $"<b>{tempSushi.Name}</b>\n{tempSushi.ShowCategory}\n{tempSushi.Description}\n" +
                                $"Weight: {tempSushi.Weight} g\nPortion: {tempSushi.MinLot} pcs\nPrice: {tempSushi.Price} BYN\n{tempCartItem}\n"+
                                $"Would you like to modify product quantity in the cart?\nInput integer number of servings you wish or 0 to remove completely .",
                                parseMode: ParseMode.Html,
                                replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd)
                               );
                            return;

                         case Pizza tempPizza:
                            await botClient.SendPhotoAsync(
                                chatId: id,
                                photo: tempPizza.PictureURL,
                                caption: $"<b>{tempPizza.Name}</b>\n{tempPizza.ShowCategory}\n{tempPizza.Description}\n" +
                                $"Weight: {tempPizza.Weight} g\nPortion: {tempPizza.Diameter} cm\nPrice: {tempPizza.Price} BYN\n{tempCartItem}\n" +
                                $"Would you like to modify product quantity in the cart?\nInput integer number of servings you wish or 0 to remove completely .",
                                parseMode: ParseMode.Html,
                                replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd)
                               );
                            return;

                        case Drink tempDrink:
                            await botClient.SendPhotoAsync(
                                chatId: id,
                                photo: tempDrink.PictureURL,
                                caption: $"<b>{tempDrink.Name}</b>\n{tempDrink.ShowCategory}\n{tempDrink.Description}\n" +
                                $"Volume: {tempDrink.Volume} ml\nPrice: {tempDrink.Price} BYN\n{tempCartItem}\n" +
                                $"Would you like to modify product quantity in the cart?\nInput integer number of servings you wish or 0 to remove completely .",
                                parseMode: ParseMode.Html,
                                replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd)
                               );
                            return;

                        default:
                            await botClient.SendPhotoAsync(
                                  chatId: id,
                                  photo: tempProd.PictureURL,
                                  caption: $"{tempProd.ToString()}\n" +
                                  "Would you like to modify product quantity in the cart?\nInput integer number of servings you wish 0 to remove completely.",
                                   replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));

                            //await botClient.SendPhotoAsync(
                            //    chatId: id,
                            //    photo: tempProd.PictureURL,
                            //    caption: $"<b>{tempProd.Name}</b>\n{tempProd.ShowCategory}\n{tempProd.Description}\n" +
                            //    $"Weight: {tempProd.Weight} g\nPrice: {tempProd.Price} BYN\n{tempCartItem}\n" +
                            //    $"Would you like to modify product quantity in the cart?\nInput integer number of servings you wish 0 to remove completely.",
                            //    parseMode: ParseMode.Html,
                            //    replyMarkup: KbdMaker.InKeyboard(CommandNames.SystemKbd));
                            return;
                    }
                }

                if (msg.StartsWith("/delcartitem"))
                {
                   await DelCartItemCmd<T>.Command(botClient, id, msg, firstName, lastName);
                   return;
                }
            }
        }
    }
}
