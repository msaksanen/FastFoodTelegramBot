using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot.Repositories
{
    class CmdUserStatusRepos<T>  where T: CommandUserStatus
    {
        public static List<T> Repos { get; set; } = new List<T>(25);

        public static CommandUserStatus GetStatusById(long id)
        {
            CommandUserStatus? st=Repos.Find(p => p.ChatId == id);
            if (st==null)
            {
                st = new() { ChatId = id, NextCmd= CommandStatus.Intro, LastCmd=CommandStatus.Start };
                Repos.Add((T)st);
            }
            return st;
        }
    }
}
