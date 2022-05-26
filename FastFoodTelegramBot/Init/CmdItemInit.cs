using FastFoodTelegramBot.Commands;
using FastFoodTelegramBot.Models;
using FastFoodTelegramBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FastFoodTelegramBot.CommandNames;

namespace FastFoodTelegramBot.Init
{
    class CmdItemInit
    {
        public static Dictionary<string, string> MainProdCatBtn = new();
        public static void CmdInit()
        {
            CmdItem[] cmdItems = new CmdItem[50];
            cmdItems[0] = new() { CmdExec = StartCmd.Command, PrefKeyCmd = "/start", PrefNextCmd = CommandStatus.Intro, BtnCmd = CommandNames.IntroBtnCmd };
            cmdItems[1] = new() { CmdExec = RegMainCmd.Command, PrefKeyCmd = "/reg", PrefNextCmd = CommandStatus.Reg, BtnCmd = CommandNames.RegBtnCmd };
            cmdItems[2] = new() { CmdExec = RestartBotCmd.Command, PrefKeyCmd = "/restart", PrefKeyCmd2 = "restart bot", PrefNextCmd = CommandStatus.Start, BtnCmd = CommandNames.IntroBtnCmd };
            cmdItems[3] = new() { CmdExec = ViewCmd.Command, PrefKeyCmd = "/view", PrefNextCmd = CommandStatus.View, BtnCmd = MainProdCatBtn };
            cmdItems[4] = new() { CmdExec = UpLevelCmd.Command, PrefKeyCmd = "/up", PrefKeyCmd2 = "upper menu", PrefNextCmd = CommandStatus.Up };
            cmdItems[5] = new() { CmdExec = FirstNameRegCmd.Command, PrefKeyCmd = "/firstname", PrefNextCmd = CommandStatus.FirstName };
            cmdItems[6] = new() { CmdExec = FirstNameInpCmd.Command, PrefNextCmd = CommandStatus.FirstNameInp, BtnCmd = CommandNames.RegBtnCmd };
            cmdItems[7] = new() { CmdExec = LastNameRegCmd.Command, PrefKeyCmd = "/lastname", PrefNextCmd = CommandStatus.LastName };
            cmdItems[8] = new() { CmdExec = LastNameInpCmd.Command, PrefNextCmd = CommandStatus.LastNameInp, BtnCmd = CommandNames.RegBtnCmd };
            cmdItems[9] = new() { CmdExec = EmailRegCmd.Command, PrefKeyCmd = "/email", PrefNextCmd = CommandStatus.Email };
            cmdItems[10] = new() { CmdExec = EmailInpCmd.Command, PrefNextCmd = CommandStatus.EmailInp, BtnCmd = CommandNames.RegBtnCmd };
            cmdItems[11] = new() { CmdExec = PayMethodCmd.Command, PrefKeyCmd = "/paymethod", PrefNextCmd = CommandStatus.PayMethod, BtnCmd = CommandNames.PayBtnCmd };
            cmdItems[12] = new() { CmdExec = PayCashCmd.Command, PrefKeyCmd = "/cash", PrefNextCmd = CommandStatus.PayCash, BtnCmd = CommandNames.PayBtnCmd };
            cmdItems[13] = new() { CmdExec = PayCardCmd.Command, PrefKeyCmd = "/card", PrefNextCmd = CommandStatus.PayCard, BtnCmd = CommandNames.PayBtnCmd };
            cmdItems[14] = new() { CmdExec = RegInfoCmd.Command, PrefKeyCmd = "/reginfo", PrefNextCmd = CommandStatus.Reg, BtnCmd = CommandNames.RegBtnCmd };
            cmdItems[15] = new() { CmdExec = UpLevelCmd.Command, PrefKeyCmd = "/finreg", PrefNextCmd = CommandStatus.Up };
            cmdItems[16] = new() { CmdExec = ItemToCartCmd<Product>.Command, PrefNextCmd = CommandStatus.AdditemToCart, BtnCmd = CommandNames.SystemKbd };
            cmdItems[17] = new() { CmdExec = ItemQtyToCartCmd<Product>.Command, PrefNextCmd = CommandStatus.ItemQtyToCart, BtnCmd = CommandNames.SystemKbd };
            cmdItems[18] = new() { CmdExec = ViewCartCmd<Product>.Command, PrefKeyCmd = "/viewcart", PrefKeyCmd2 = "view cart", PrefNextCmd = CommandStatus.ViewCart };
            cmdItems[19] = new() { CmdExec = ModifyCartCmd<Product>.Command, PrefKeyCmd = "/modcart", PrefKeyCmd2 = "modify cart", PrefNextCmd = CommandStatus.ModifyCartItem, BtnCmd = CommandNames.ModifyCartBtnCmd };
            cmdItems[20] = new() { CmdExec = ModifyCartItemInpCmd<Product>.Command, PrefNextCmd = CommandStatus.ModifyCartItemInp, BtnCmd = CommandNames.SystemKbd };
            cmdItems[21] = new() { CmdExec = ModifyCartItemInp2Cmd<Product>.Command, PrefNextCmd = CommandStatus.ModifyCartItemInp2, BtnCmd = CommandNames.SystemKbd };
            cmdItems[22] = new() { CmdExec = OrderCmd.Command, PrefKeyCmd = "/order", PrefKeyCmd2 = "order", PrefNextCmd = CommandStatus.Order, BtnCmd = CommandNames.OrderBtnCmd };
            cmdItems[23] = new() { CmdExec = MakeOrderCmd.Command, PrefKeyCmd = "/makeorder", PrefKeyCmd2 = "make order", PrefNextCmd = CommandStatus.MakeOrder, BtnCmd = CommandNames.OrderBtnCmd };


            foreach (CmdItem item in cmdItems)
            {
                if (item != null)
                    CmdListRepository<CmdItem>.Repos.Add(item);
            }

            // CmdListRepository<CmdItem>.Repos.AddRange(cmdItems);


            ProdCategory[] prodCat = new ProdCategory[10];

            prodCat[0] = new() { Name = "Sushi", PrefKeyCmd = "/sushi" };
            ProdSubCategory[] prodSubcat = new ProdSubCategory[10];
            prodSubcat[0] = new() { Name = "Sushi Classic", PrefKeyCmd = "/sushi-classic" };
            prodSubcat[1] = new() { Name = "Sushi Baked", PrefKeyCmd = "/sushi-baked" };
            prodSubcat[2] = new() { Name = "Sushi Tempura", PrefKeyCmd = "/sushi-tempura" };
            prodSubcat[3] = new() { Name = "Sushi Sets", PrefKeyCmd = "/sushi-sets" };
            prodSubcat[4] = new() { Name = "Sushi Sauces", PrefKeyCmd = "/sushi-sauces" };

            foreach (ProdSubCategory item in prodSubcat)
            {
                if (item != null)
                    prodCat[0].SubCatList.Add(item);
            }
            // prodCat[0].SubCatList.AddRange(prodSubcat);

            prodCat[1] = new() { Name = "Pizza", PrefKeyCmd = "/pizza" };
            prodCat[2] = new() { Name = "Burger", PrefKeyCmd = "/burger" };
            prodCat[3] = new() { Name = "Fried", PrefKeyCmd = "/fried" };
            prodCat[4] = new() { Name = "Beverages", PrefKeyCmd = "/drink" };

            //CommandStatus newStatus = new();
            //string[] RealCmdStatus = newStatus.GetType().GetEnumNames();

            //Type statusType = typeof(CommandStatus);
            //int enumLength=statusType.GetEnumNames().Length;

            foreach (ProdCategory item in prodCat)
            {
                if (item != null)
                    MainProdCatBtn.Add(item.PrefKeyCmd, item.Name);
            }
            MainProdCatBtn.Add("/up", "Return to upper level");
            MainProdCatBtn.Add("/restart", "Restart bot");


            foreach (ProdCategory item in prodCat)
            {
                if (item != null)
                    ProdCatRepository.CatList.Add(item);
            }
            //  ProdCatRepository.CatList.AddRange(prodCat);


            foreach (ProdCategory item in prodCat)
            {
                if (item != null)
                {
                    CmdItem tempcmdItem = new() { CmdExec = ProdCatCmd.Command, PrefKeyCmd = item.PrefKeyCmd, PrefNextCmd = CommandStatus.ProdCat, PrefNextCmdStr = item.Name, BtnCmd = item.ProdSubCatBtn };
                    CmdListRepository<CmdItem>.Repos.Add(tempcmdItem);
                    foreach (ProdSubCategory subitem in item.SubCatList)
                    {
                        if (subitem != null)
                        {
                            item.ProdSubCatBtn.Add(subitem.PrefKeyCmd, subitem.Name);
                            CmdItem tempsubItem = new() { CmdExec = ProdSubCatCmd.Command, PrefKeyCmd = subitem.PrefKeyCmd, PrefNextCmd = CommandStatus.ProdSubCat, PrefNextCmdStr = subitem.Name, BtnCmd = CommandNames.ItemToCartBtnCmd };
                            CmdListRepository<CmdItem>.Repos.Add(tempsubItem);
                        }
                    }
                    item.ProdSubCatBtn.Add("/up", "Return to upper level");
                    item.ProdSubCatBtn.Add("/restart", "Restart bot");
                }
            }
        }
    }
}
