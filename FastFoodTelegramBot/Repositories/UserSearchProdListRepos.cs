using FastFoodTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot.Repositories
{
    class UserSearchProdListRepos <SearchProdListRepository,T> where T: Product
    {
        public static List<SearchProdListRepository<T>> Repos { get; set; } = new List<SearchProdListRepository<T>>(25);
        public static SearchProdListRepository<T> GetSearchProdListById(long id)
        {
            SearchProdListRepository<T>? usrsprod = Repos.Find(p => p.ChatId == id);
            if (usrsprod == null)
            {
                usrsprod = new() { ChatId = id};
                Repos.Add(usrsprod);
            }
            return usrsprod;
        }
    }
}
