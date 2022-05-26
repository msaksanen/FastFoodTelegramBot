using FastFoodTelegramBot.Commands;
using FastFoodTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Repositories
{
    public class CmdListRepository <T>
    {
        public static List<T> Repos { get; set; } = new List<T>(60);
    }
}
