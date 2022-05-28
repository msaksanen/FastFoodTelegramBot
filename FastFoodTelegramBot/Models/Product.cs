using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    
    public class Product : BaseElement
    {
        public string CategoryCmd { get; set; }
        public string SubCategory { get; set; }
        public string GeneralCategory { get; set; }
        public string ShowCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
       // public Guid ProdId { get; } //= Guid.NewGuid();
        public int Weight { get; set; }
        public float Price { get; set; }
        public float PromoPrice { get; set; }
        public bool IsPromo { get; set; }
        public string PromoCondition { get; set; }
        public int ShowPicInCat { get; set; } = 1;
        public string AddProperty { get; set; }

        //public Product():base()
        //{
        //    this.Id = this.DbId;
        //}
        public override string ToString()
        {
            return "Product category: " + ShowCategory + ".\nName: " + Name + ".\nDescription: " + Description  +"\nWeight: " + Weight + " gr.\n" + AddProperty+ "\nPrice: " + Price + " BYN.";
        }
    }
}
