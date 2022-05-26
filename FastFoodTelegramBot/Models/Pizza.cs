using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    class Pizza: Product
    {
        public int Diameter { get; set; }

        public override string ToString()
        {
            return "Product category: " + ShowCategory + ".\nName: " + Name + ".\nDescription: " + Description + "\nWeight: " + Weight + " gr.\nDiameter: " + Diameter + "cm.\nPrice: " + Price + " BYN.";
        }
    }
}
