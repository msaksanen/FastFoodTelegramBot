using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Mail;
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
using System.Net;
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot.Commands
{
    class MakeOrderCmd:ICommand
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
                if (foundAccount.Email == string.Empty || foundAccount.Email == null)
            {
                st.NextCmd = CommandStatus.EmailInp;
                st.LastCmd = CommandStatus.Reg;
                st.NextCmdStr = CommandNames.InpMail;
                await botClient.SendTextMessageAsync(id, "You haven't input email.\n" + CommandNames.InpMail);
                return;
            }

            ShopCartItemRepository<ShopCartItem<Product>> usrshopcart = UserShopCartItemRepos<ShopCartItemRepository<ShopCartItem<Product>>, ShopCartItem<Product>>.GetShopCartItemListById(id);
            Order<ShopCartItem<Product>, Product> order = new() { ChatID = foundAccount.ChatId }; // ShopCart = usrshopcart.Repos
            foreach (var item in usrshopcart.Repos)
            {
                item.OrderId = order.Id;
            }
            order.ShopCart = usrshopcart.Repos;
            // OrderRepository<Order<ShopCartItem<Product>, Product>>.Repos.Add(order);
            CallBackFunc.@AddOrderInDb(order, OrderRepository<Order<ShopCartItem<Product>, Product>>.Repos);

            StringBuilder sb = new StringBuilder();
            float totalprice = 0;
            string tempstr;
            st.LastCmd = CommandStatus.View;
            sb.AppendLine($"Order № {order.Id}");
            sb.AppendLine($"Creation Time: {order.Created}");
            sb.AppendLine("============================");
            sb.AppendLine(foundAccount.ToString());
            sb.AppendLine("============================");
            sb.AppendLine("Selected products");
            sb.AppendLine("============================");

            for (int i = 0; i < usrshopcart.Repos.Count; i++) //(int i = 0; i < st.ShopCartItemQty; i++)
            {
                sb.AppendLine(usrshopcart.Repos[i].ToString());
                sb.AppendLine("============================");
                totalprice += usrshopcart.Repos[i].PositionPrice();
            }

            string tprice = string.Format("{0:f2}", totalprice+order.AdditionPrice);
            sb.AppendLine($"Total price: {tprice} BYN");
            sb.AppendLine("============================");
          
            string subject = $"FastFoodBot: Order created - {order.Created}.";
            tempstr = "Sales check has been sent to your email.\n The order will be packaged soon.";

            //await EmailSender.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            //await EmailSendGrid.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            await CallBackFunc.@EmailSend(foundAccount, subject, sb.ToString() + tempstr);

            if (st.isMailSent)
            {
                //tempstr="Sales check has been sent to your email.\n The order will be delivered to your address.";
                await botClient.SendTextMessageAsync(id, $"{sb}\n{tempstr}",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
            }
            else
            {
                tempstr = "Order status notification wasn't sent to your email.\n Check your email address in \"Registration data\" or contact with our Support Service.";
                await botClient.SendTextMessageAsync(id, $"{sb}\n{tempstr}",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
                return;
            }

            Thread.Sleep(2000);

            subject = $"FastFoodBot: Order packaged - {order.Created}";
            tempstr = "Your order has been packaged.\n The order will be delivered to your address soon.";
            //await EmailSender.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            //await EmailSendGrid.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            await CallBackFunc.@EmailSend(foundAccount, subject, sb.ToString() + tempstr);

            if (st.isMailSent)
            {
               
                await botClient.SendTextMessageAsync(id, $"{tempstr}",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
            }
            else
            {
                tempstr = "Order status notification wasn't sent to your email.\n Check your email address in \"Registration data\" or contact with our Support Service.";
                await botClient.SendTextMessageAsync(id, $"{sb}\n{tempstr}",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
                return;
            }

            Thread.Sleep(3000);

            subject = $"FastFoodBot: Order deliveried - {order.Created}";
            tempstr = "The order has been successfully delivered and paid-up.";
            //await EmailSender.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            //await EmailSendGrid.SendEmailAsync(foundAccount, subject, sb.ToString() + tempstr);
            await CallBackFunc.@EmailSend(foundAccount, subject, sb.ToString() + tempstr);

            if (st.isMailSent)
            {

                await botClient.SendTextMessageAsync(id, $"{tempstr} \nThe cart will be cleared. \nThe bot will be restarted. ",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
            }
            else
            {
                tempstr = "Order status notification wasn't sent to your email.\n Check your email address in \"Registration data\" or contact with our Support Service.";
                await botClient.SendTextMessageAsync(id, $"{sb}\n{tempstr}",
                      replyMarkup: KbdMaker.InKeyboard(CommandNames.OrderBtnCmd));
                return;
            }
            await RestartBotCmd.Command(botClient, id, msg, firstName, lastName);

        }
    }
}
