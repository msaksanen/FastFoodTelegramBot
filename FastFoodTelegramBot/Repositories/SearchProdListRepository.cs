using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Repositories
{
    public class SearchProdListRepository<T>
    {
        public long ChatId { get; set; }
        public List<T> Repos { get; set; } = new List<T>(50);

        string _path = @"D:\prodsearchRepos.json";

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
