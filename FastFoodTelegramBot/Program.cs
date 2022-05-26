using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastFoodTelegramBot.Repositories;
using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Commands;
using static FastFoodTelegramBot.CommandNames;
using System.Reflection;
using System.Reflection.Emit;
using FastFoodTelegramBot.Utilities;
using FastFoodTelegramBot.Services;
using Microsoft.EntityFrameworkCore;
using FastFoodTelegramBot.Init;

namespace FastFoodTelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            CallBackFunc.Args = args;

            CallBackFunc.Init();
            LogHelper.Debug($"Source mode:{CommandNames.CmdArg}", $"Log level: {CommandNames.LogLevel}. Email Service: {CommandNames.EmailService}.");
                       
            CmdItemInit.CmdInit();
            ProductReposInit.ProductInit();

            ProductListRepository<Product>.Repos=CallBackFunc.@LoadProdFileToList?.Invoke(ProductListRepository<Product>.Repos); //Works in json mode. In list Mode is empty.
            CallBackFunc.@SaveProdListToFile?.Invoke(ProductListRepository<Product>.Repos); //Works in list mode. In json Mode is empty.
           
            TelegramService.TelegramStart();

            Console.ReadLine();
        }
    }
}
