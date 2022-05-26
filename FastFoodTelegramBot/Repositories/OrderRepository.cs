using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Repositories
{
    class OrderRepository<T>
    {
        public static List<T> Repos { get; set; } = new List<T>(50);

        static string _path = "order.json";

        public static string Path
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
