using FastFoodTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Repositories
{
    public class ShopCartItemRepository<ShopCartItem> : BaseElement
    {
        public long ChatId { get; set; }
        public List<ShopCartItem> Repos { get; set; } = new List<ShopCartItem>(50);

        string _path = @"D:\shopcartRepos.json";

        public string Path
        {
            get { return _path; }

            set
            {
                if (value.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine("Input correct file name with path. Empty field is prohibited.");
                else _path = value;
            }
        }
    }
}
