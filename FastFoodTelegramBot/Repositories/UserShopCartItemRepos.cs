using FastFoodTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot.Repositories
{
    class UserShopCartItemRepos <ShopCartItemRepository,ShopCartItem>
    {
        public static List<ShopCartItemRepository<ShopCartItem>> Repos { get; set; } = new List<ShopCartItemRepository<ShopCartItem>>(25);
        public static ShopCartItemRepository<ShopCartItem> GetShopCartItemListById(long id)
        {
            ShopCartItemRepository<ShopCartItem>? usrshopcart = Repos.Find(p => p.ChatId == id);
            if (usrshopcart == null)
            {
                usrshopcart = new() { ChatId = id };
                Repos.Add(usrshopcart);
            }
            return usrshopcart;
        }
    }
}
