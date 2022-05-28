using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static FastFoodTelegramBot.Services.CmdService;

namespace FastFoodTelegramBot
{
    public class CommandNames
    {
        public const string InpNameF = "Input your first name, please.";
        public const string InpNameL = "Input your last name, please.";
        public const string InpMail = "Input your e-mail, please.";
        public const string InpPayMethod = "Choose your paymethod, please.";
        public static string ServiceGmail { get; set; } = "";
        public static string ServicePwd { get; set; } = "x";
        public static string SendGridApiKey { get; set; } = "SG";
        public static string ServiceMailField { get; set; } = "";
        public static string ServiceMailFrom { get; set; } = "FastFood Service";
        public static string TGToken { get; set; } = "";
        public static string JsonProdPath { get; set; }  = ProductListRepository<Product>.Path;
        public static string JsonAccPath { get; set; }   = AccListRepos<Account>.Path;
        public static string JsonOrderPath { get; set; } = OrderRepository<Order<ShopCartItem<Product>, Product>>.Path;
     
        //public static string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|AppData\dbContext.mdf';Integrated Security=True";
        public static string AccConnectionDB { get; set; } = @"Server=(localdb)\mssqllocaldb;AttachDbFilename='|DataDirectory|AppData\FFoodAccdb.mdf';Database=FFoodAccdb;Trusted_Connection=True;";
        public static string ProdConnectionDB { get; set; } = @"Server=(localdb)\mssqllocaldb;AttachDbFilename='|DataDirectory|AppData\FFoodProddb.mdf';Database=FFoodProddb;Trusted_Connection=True;";
        public static string OrderConnectionDB { get; set; } = @"Server=(localdb)\mssqllocaldb;AttachDbFilename='|DataDirectory|AppData\FFoodOrderdb.mdf';Database=FFoodOrderdb;Trusted_Connection=True;";


        public static string CmdArg { get; set; } = "list";
        public static string LogLevel { get; set; } = "info";
        public static string EmailService { get; set; } = "Gmail";
        public enum CommandStatus
        {
           Start,
           Intro,
           Reg,
           View,
           Up,
           FirstName,
           FirstNameInp,
           LastName,
           LastNameInp,
           Email,
           EmailInp,
           PayMethod,
           PayCash,
           PayCard,
           ProdCat,
           ProdSubCat,
           //Sushi,
           //Pizza,
           //Burger,
           //Fried,
           //Drink,
           SearchList,
           AdditemToCart,
           ItemQtyToCart,
           ViewCart,
           ModifyCartItem,
           ModifyCartItemInp,
           ModifyCartItemInp2,
           Order,
           MakeOrder
        }

        public static Dictionary<string, string> StartBtnCmd = new Dictionary<string, string>()
        {
            ["/start"] = "Starting bot"
        };

        public static Dictionary<string, string> IntroBtnCmd = new Dictionary<string, string>()
        {
            ["/reg"] = "Input registration data",
            ["/view"] = "View our catalogue",
            ["/restart"] = "Restart bot",
          //  ["/start"] = ""  //	Just preceding command must be incuded to avoid cycling in CmdService when it tries to resolve CommandStatus/msg mismatch.
        };

        public static Dictionary<string, string> RegBtnCmd = new Dictionary<string, string>()
        {
            ["/email"] = "Input your e-mail",
            ["/paymethod"] = "Input payment method",
            ["/firstname"] = "Input/Modify your First Name",
            ["/lastname"] = "Input/Modify your Last Name",
            ["/reginfo"] = "View registration info",
            ["/finreg"] = "Finish registration",
            ["/up"] = "Return to upper level",
            ["/restart"] = "Restart bot",
           // ["/start"] = "",
            ["/reg"] = "",
            ["/card"] = "",
            ["/cash"] = "",
        };

        public static Dictionary<string, string> PayBtnCmd = new Dictionary<string, string>()
        {
            ["/card"] = "Paying method: credit card",
            ["/cash"] = "Paying method: cash",
            ["/finreg"] = "Finish registration",
            ["/up"] = "Return to upper level",
            ["/restart"] = "Restart bot",
            ["/paymethod"] ="",
           // ["/start"] = ""
        };

        public static Dictionary<string, string> OrderBtnCmd = new Dictionary<string, string>()
        {
            ["/makeorder"]="Make order",
            ["/reg"] = "Input registration data",
            ["/modcart"] = "Modify Cart",
            ["/view"] = "View our catalogue",
            ["/up"] = "Return to upper level",
            ["/restart"] = "Restart bot",
        };

        public static Dictionary<string, string> ItemToCartBtnCmd = new Dictionary<string, string>()
        {
            ["/cart"] ="Add to cart",
            ["/up"] = "Return to upper level",
            ["/restart"] = "Restart bot",
           // ["/start"] = "",
    
        };

        public static Dictionary<string, string> ModifyCartBtnCmd = new Dictionary<string, string>()
        {
            ["/modcartitem"] = "Modify cart item",
            ["/delcartitem"] = "Delete cart item",
            ["/up"] = "Return to upper level"
            //["/start"] = "",

        };

        public static Dictionary<string, string> SystemKbd = new Dictionary<string, string>()
        {
            ["/up"] = "Return to upper level",
            ["/restart"] = "Restart bot",
            //["/start"] = ""

        };


    }
}
