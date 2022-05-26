using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using FastFoodTelegramBot.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Services 
{
    class JsonService<T>
    {
        //public static string PathProd { get; set; }
        public static List<T> LoadFileToList(List<T> list, string reposname)
        {
            string pathToDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");
            string pathToRepos = Path.Combine(pathToDir, reposname);
            try
            {
                using StreamReader file = File.OpenText(pathToRepos);
                JsonSerializer serializer = new JsonSerializer();
                string str = string.Format($"Object database loaded from Repository JSON file at: " + pathToRepos);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"JSON mode LoadFileToList(). List type : {list}");
                return (List<T>)serializer.Deserialize(file, typeof(List<T>));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
                return list;
            }

        }

        public static void SaveListToFile(List<T> list, string reposname)
        {
            string pathToDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");
            string pathToRepos= Path.Combine(pathToDir, reposname);
            try
            {
                if (!Directory.Exists(pathToDir))
                {
                    Directory.CreateDirectory(pathToDir);
                    LogHelper.Debug($"JSON mode SaveListToFile()", $"Target directory created: {pathToDir}");
                }

                var serializer = new JsonSerializer();
                using StreamWriter fs = new StreamWriter(pathToRepos);
                using var jsonTextWriter = new JsonTextWriter(fs);
                serializer.Serialize(fs, list);
                string str = string.Format($"Object database List {list} saved to repository JSON file at: " + pathToRepos);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"JSON mode SaveListToFile(). List type : {list}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }

        }

        public static void ClearJSON(string reposname)
        {
            string pathToDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");
            string pathToRepos = Path.Combine(pathToDir, reposname);
            try
            {
                File.WriteAllText(pathToRepos, string.Empty);
                string str = string.Format($"JSON-related file {pathToRepos} cleared.");
                Console.WriteLine(str);
                LogHelper.Debug(str, $"JSON mode ClearJSON().");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }
        }

        public static void AddItemInDb(T item, List<T> list, string reposname)
        {
            string pathToDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppData");
            string pathToRepos = Path.Combine(pathToDir, reposname);
            string name = item.GetType().Name;
            string str = default;
            PropertyInfo? id = item.GetType().GetProperty("Id");
            var ID_val = id.GetValue(item);
            try
            {
                list = LoadFileToList(list,reposname);
                list.Add(item);
                SaveListToFile(list,reposname);
                str = string.Format($"New object {item}  with ID {ID_val} is created and put in Repository JSON file at: " + pathToRepos);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"JSON mode. AddItemInDb(). List type: {list}. Item: {item}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"A provided file {reposname} Repository List or an item {item} have different types and/or " +
                                  $"item can not be put in the JSON Repository List.");
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }
        }

        public static T? GetByID(List<T> list, string reposname, object id, out bool isFound)
        {
            T? itemtemp = default;
            bool isfound = default;
            string str = default;

            list.Clear();
            list = LoadFileToList(list, reposname);

                try
                {
                    itemtemp = list.Find(p =>p.GetType().GetProperty("Id").GetValue(p).ToString() == id.ToString());
                if (itemtemp == null)
                {
                    itemtemp = list.Find(p => p.GetType().GetProperty("ChatId").GetValue(p).ToString() == id.ToString());
                }
                //itemtemp = list.Find(p => p.Id == id);
                    if (itemtemp != null)
                    {
                    str=string.Format($"GetByID: object with ID {id}  is found. It's {itemtemp.GetType().Name}.");
                    Console.WriteLine(str);
                    LogHelper.Debug(str, $"JSON mode. GetByID(). List type: {list}. Object ID: {id}.");
                    isfound = true;
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
            isFound = isfound;
            return itemtemp;
        }

        public static bool UpdateItem(T item, List<T> lst, string reposname)
        {
            string name = item.GetType().Name;
            string str = default;
            string idflag = "Id";

            lst.Clear();
            lst = LoadFileToList(lst, reposname);

            if (name.Equals("Account", StringComparison.OrdinalIgnoreCase))
               {
                idflag = "ChatId";
               }
            var itemId = item.GetType().GetProperty(idflag).GetValue(item);
           // var chatitemId = item.GetType().GetProperty("Id").GetValue(item);

            int x = -1;
                for (int i = 0; i < lst.Count; i++)
                {
                   var lstId=lst[i].GetType().GetProperty(idflag).GetValue(lst[i]);
                   if (itemId.ToString().Equals(lstId.ToString(), StringComparison.OrdinalIgnoreCase))  x = i;
                }
                try
                {
                    if (x != -1)
                    {
                        str=string.Format($"UpdateItem: object with ID {itemId} is found in database. It's {name}. " +
                                          $"It will be replaced with your updated object.");
                        Console.WriteLine(str);
                        LogHelper.Debug(str, $"JSON mode.UpdateItem(). List type: {lst}. Item: {item}.");
                        lst[x] = item;
                        SaveListToFile(lst, reposname);
                        return true;
                    }
                    else throw new ItemNotFoundException($"Update Item. Object with ID {itemId} is not found.");
                }
                catch (ItemNotFoundException e)
                {
                    Console.WriteLine($"Exception: {e.Message}");
                    Console.WriteLine($"Exception: {e.StackTrace}");
                    Console.WriteLine($"A provided file {reposname} Repository List or an item {item} have different types and/or " +
                                      $"item can not be put in the JSON Repository List.");
                    LogHelper.Error(e);
            }
          
            return false;
        }
    }
}
