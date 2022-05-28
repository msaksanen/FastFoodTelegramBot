using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using System.Reflection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodTelegramBot.Utilities;

namespace FastFoodTelegramBot.Services
{
    class DbService<T> where T:BaseElement
    {

        public static void SaveListToDB(List<T> list, ApplicationContext<T> db)
        {
            string dbname = db.Database.GetConnectionString();
            string listname = list.GetType().Name;
            string str = default;
            try
            {
                db.SQL.RemoveRange(db.SQL);
                db.SQL.AddRange(list);
                db.SaveChanges();
                //var dblist = db.SQL.ToList();
                //var result = dblist.Union(list);
                ////// var data = (from n in db.SQL select n);

                // if (dblist != null)
                // {
                //     db.SQL.RemoveRange(db.SQL);
                //     db.SaveChanges();
                //     //db.SQL.RemoveRange(data);
                //     //db.SaveChanges();
                // }

                // db.SQL.AddRange(result);
                // db.SaveChanges();
                str=string.Format($"List {listname} has been saved to DBase file at:\n" + dbname);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"DBase mode SaveListToDB(). List type: {list}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"A provided DBase {dbname}\n or a List {listname} have different types and/or " +
                                  $"items can not be put in the DB.");
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }
        }

        public static List<T> LoadDBToList(List<T> list, ApplicationContext<T> db)
        {
            string dbname = db.Database.GetConnectionString();
            string listname = list.GetType().Name;
            string str = default;
            List<Product> sortprodlist = default;

            try
            {
                //list.Clear();
                var lst = db.SQL.ToList();
                var sel = lst.FindAll(p => p.GetType().Name == "Product");
                    
                if (sel!=null && sel is List<Product> prodlist)
                {
                    prodlist.Sort(delegate(Product prod1, Product prod2) { return prod1.Name.CompareTo(prod2.Name); } );
                    sortprodlist = prodlist;
                }
                if (sortprodlist != null && sortprodlist is List<T> templist)
                    lst = templist;

                str = string.Format($"Object database loaded to List {listname} from DBase file at: " + dbname);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"DBase mode LoadDBToList(). List type: {list}.");
                return lst;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
                return list;
            }

        }

        public static void AddItemInDb(T item, ApplicationContext<T> db)
        {
            string name = item.GetType().Name;
            PropertyInfo? id = item.GetType().GetProperty("Id");
            var ID_val = id.GetValue(item);
            string dbname = db.Database.GetConnectionString();
            string str = default;
            try
            {
                db.SQL.Add(item);
                db.SaveChanges();
                str = string.Format($"New object {name}  with ID {ID_val} is created and put in DBase file at:\n" + dbname);
                Console.WriteLine(str);
                LogHelper.Debug(str, $"DBase mode AddItemInDb(). Item type: {item}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"A provided DBase {dbname}\n or an item {name} have different types and/or " +
                                  $"item can not be put in the DB.");
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                LogHelper.Error(e);
            }
        }

        public static T? GetByID(ApplicationContext<T> db, object id, out bool isFound)
        {
            T? itemtemp = default;
            bool isfound = default;
            string str = default;
            string dbname = db.Database.GetConnectionString();

            try
            {
                var lst = db.SQL.ToList();
                itemtemp = lst.Find(p => p.GetType().GetProperty("Id").GetValue(p).ToString() == id.ToString());
                if (itemtemp == null)
                {
                    itemtemp = lst.Find(p => p.GetType().GetProperty("ChatId").GetValue(p).ToString() == id.ToString());
                }
                //int x = -1;
                //for (int i = 0; i < lst.Count; i++)
                //{
                //    var lstId = lst[i].GetType().GetProperty("Id").GetValue(lst[i]);
                //    var lstchatId = lst[i].GetType().GetProperty("ChatId").GetValue(lst[i]);
                //    if (id.ToString().Equals(lstId.ToString(), StringComparison.OrdinalIgnoreCase)) x = i;
                //    if (id.ToString().Equals(lstchatId.ToString(), StringComparison.OrdinalIgnoreCase)) x = i;
                //}

                //itemtemp = templist.Find(p => p.GetType().GetRuntimeProperty("Id").GetValue(p).ToString() == id.ToString());
                //itemtemp = list.Find(p => p.GetType().GetProperty("Id").GetValue(p).ToString() == id.ToString());
                //itemtemp = list.Find(p => p.Id == id);
                if (itemtemp != null) //(x != -1) //
                {
                    isfound = true;
                    str =string.Format($"GetByID: object with ID {id} is found. It's {itemtemp.GetType().Name}.");
                    Console.WriteLine(str);
                    LogHelper.Debug(str, $"DBase mode GetByID(). Dbase:{dbname}");  
                   // itemtemp = lst[x];
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

        public static bool UpdateItem(T item, ApplicationContext<T> db)
        {
            string itemname = item.GetType().Name;
            string idflag = "Id";
            string str = default;

            if (itemname.Equals("Account", StringComparison.OrdinalIgnoreCase))
            {
                idflag = "ChatId";
            }
            var itemId = item.GetType().GetProperty(idflag).GetValue(item);
            var templist = db.SQL.ToList();
            T? itemold = templist.Find(p => (p.GetType().GetProperty(idflag).GetValue(p).ToString() == itemId.ToString()));
            string nameold = itemold.GetType().Name;
            string dbname = db.Database.GetConnectionString();

            try
            {
                if (itemold != null)
                {
                    db.SQL.Remove(itemold);
                    db.SQL.Add(item);
                    db.SaveChanges();
                    str =string.Format($"Update Item: object with ID {itemold} is found in database. It's {nameold}. It will be replaced with new object.");
                    Console.WriteLine(str);
                    LogHelper.Debug(str, $"DBase mode UpdateItem(). Dbase:{dbname}");
                    return true;
                }
                else
                    throw new ItemNotFoundException($"Update Account:Object with ID {itemId} is not found in database.");
            }
            catch (ItemNotFoundException e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                Console.WriteLine($"Exception: {e.StackTrace}");
                Console.WriteLine($"A provided DBase {dbname}\n or an item {item} have different types and/or " +
                                   $"item can not be put in the DB.");
                LogHelper.Error(e);
            }

            return false;
        }

    }
}
