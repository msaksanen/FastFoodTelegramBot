using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FastFoodTelegramBot.Services
{
    public static class ListAccService<T>
    {
        public static void AddItemInDb(T item, List<T> list)
        {
            string str = default;
            if (list is List<Account> lst && item is Account acc)
            {
                lst.Add(acc);
                str = string.Format($"New Account object {acc.FirstName}  with ID {acc.Id} is created and put in the Repository List.");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"List Mode: AddItemInDb(). User Chat Id:{acc.Id}.");
            }
            else
            {
                str = string.Format($"A provided List {list} is not an Account Repository List or an object {item}\n is not an account and can not be put in the Repository List.");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"List Mode: AddItemInDb(). User Chat Id: not provided.");
            }
        }
        public static Account GetByID(List<T> list, long id, out bool isFound)
        {
            Account acctemp = default;
            bool isfound = default;
            string str = default;
            if (list is List<Account> lst)
            {
                try
                {
                    acctemp = lst.Find(p => p.ChatId == id);
                    if (acctemp != default)
                    {
                        str = string.Format($"GetByID: object with ID {id} is found. It's {acctemp?.FirstName}.");
                        Console.WriteLine(str);
                        isfound = true;
                        LogHelper.Debug(str, $"List Mode: GetByID(). User Chat Id:{id}.");
                    }
                    else
                        throw new ItemNotFoundException($"GetByID. Object with ID {id} is not found.");
                }
                catch (ItemNotFoundException e)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                    Console.WriteLine($"Exception: {e.StackTrace}");
                    LogHelper.Error(e);
                }

            }
            else
            {
                str = string.Format($"A provided List {list} is not a Account Repository List.");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"List Mode: GetByID(). Provided Chat Id:{id}.");
            }
            isFound = isfound;
            return acctemp;
        }

    }
}
