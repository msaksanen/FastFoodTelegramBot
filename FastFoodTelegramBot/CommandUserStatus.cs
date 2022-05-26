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
using FastFoodTelegramBot.Commands;
using static FastFoodTelegramBot.CommandNames;
using FastFoodTelegramBot.Repositories;

namespace FastFoodTelegramBot
{
    class CommandUserStatus
    {
        public long ChatId { get; set; }
        public int SearchListIndex { get; set; } = 0;
        public int SearchIndexLength { get; set; } = 0;
        public int ShopCartItemQty { get; set; } = 0;
        public string LastProdId { get; set; } = string.Empty;
        public string LastCartItemdId { get; set; } = string.Empty;
        public  CommandStatus LastCmd { get; set; } = 0;
        public CommandStatus NextCmd { get; set; } = 0;
        public string LastCmdStr { get; set; } = default;
        public string NextCmdStr { get; set; } = default;
        public bool isMailSent { get; set; } = default;
    }
}
