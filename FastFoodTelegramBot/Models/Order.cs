using FastFoodTelegramBot.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    class Order<ShopCartItem,T> : BaseElement  where T : Product
    {
        //public Guid Id { get; } //= Guid.NewGuid();
       
        public long ChatID { get; set; }

        public DateTime Created { get; } = DateTime.Now;

        public List<ShopCartItem<T>> ShopCart { get; set; } = new List<ShopCartItem<T>>(25);
        public float OrderPrice()
        {
            float price = 0f;

            foreach (var item in ShopCart)
                price += item.PositionPrice();

            return price + AdditionPrice;
        }
        public float AdditionPrice { get; set; }
        //public Order() : base()
        //{
        //    this.Id = this.DbId;
        //}

        //string _path = $"D:\\order.json";

        //public string Path
        //{
        //    get { return _path; }

        //    set
        //    {
        //        if (value.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
        //            Console.WriteLine("Input correct file name with path. Empty field is prohibited.");
        //        else _path = value +"-" + this.Id +".json";
        //    }
        //}
    }
}
