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
    public delegate Task CommandExecutor(ITelegramBotClient botClient, long id, string? msg, string? firstName, string? lastName);
    public class CmdItem
    {
        public string PrefLastCmdStr { get; set; } = default;
        public string PrefNextCmdStr { get; set; } = default;
        public string PrefKeyCmd { get; set; } = default;
        public string PrefKeyCmd2 { get; set; } = default;
        public CommandStatus PrefLastCmd { get; set; } = 0;
        public CommandStatus PrefNextCmd { get; set; } = 0;

        public Dictionary<string, string> BtnCmd = new Dictionary<string, string>();

        public CommandExecutor CmdExec;
    }
}
