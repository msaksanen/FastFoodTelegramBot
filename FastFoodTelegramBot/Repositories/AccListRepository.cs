using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    public class AccListRepos<T> //Repository List
    {

        public static List<T> Repos { get; set; } = new List<T>(25);

       static string _path = "accRepos.json";

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
