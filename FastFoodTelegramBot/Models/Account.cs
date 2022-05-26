using FastFoodTelegramBot.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace FastFoodTelegramBot.Models
{
    public class Account : BaseElement
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long ChatId { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }

        
        //public SearchProdListRepository<Product> searchProdList { get; set; } = new ();
       
        //public ShopCartItemRepository<ShopCartItem <Product>> shopCartItemList { get; set; } = new ();
       
        public override string ToString()
        {
            return "Account data. FirstName: " + FirstName + ". LastName: " + LastName + ".\n Email: " + Email + ". Payment Method: " + PaymentMethod+ ". ChatID: " + ChatId;
        }

    }
}
