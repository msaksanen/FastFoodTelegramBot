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

namespace FastFoodTelegramBot.Models
{
    class ProdCategory
    {
        public string Name { get; set; }
        public string PrefKeyCmd { get; set; }
        public List<ProdSubCategory> SubCatList { get; set; } = new List<ProdSubCategory>(10);

        public Dictionary<string, string> ProdSubCatBtn = new();
        
    }
}
