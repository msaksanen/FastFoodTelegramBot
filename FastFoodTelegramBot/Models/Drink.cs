using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    class Drink: Product
    {
        public int Volume { get; set; }
        public override string ToString()
        {
            return "Product category: " + ShowCategory + ".\nName: " + Name + "\nVolume: " + Volume + " ml.\nPrice: " + Price + " BYN.";
        }
    }
}
