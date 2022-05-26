using FastFoodTelegramBot.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    [Owned]
    public class ShopCartItem<T>  where T :Product
    {
        [ForeignKey("OrderId")]
        public Guid OrderId { get; set; } 

        public string ProducName { get; set; }

        public long ChatID { get; set; }
        public string StringProdId { get; set; }
        public Guid CartItemId { get; set; } = Guid.NewGuid();
        public uint Quantity { get; set; }

        //public ShopCartItem() : base()
        //{
        //    this.CartItemId = this.Id;

        //}
        public float PositionPrice()
        {
            T? Item=ProductListRepository<T>.Repos.Find(p => p.Id.ToString() == StringProdId);

            return Item.Price * Quantity + AdditionPrice;
        }
        public float AdditionPrice { get; set; }

        public override string ToString()
        {
            T? Item = ProductListRepository<T>.Repos.Find(p => p.Id.ToString() == StringProdId);
            string pos = string.Format("{0:f2}", PositionPrice());
            return Item.ToString() + "\nQuantity: " + Quantity + " portions.\nPosition price: " + pos + " BYN.";    
        }
    }
}
