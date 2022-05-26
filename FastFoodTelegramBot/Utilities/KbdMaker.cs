using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace FastFoodTelegramBot.Utilities
{
    class KbdMaker
    {
        static internal InlineKeyboardMarkup InKeyboard(Dictionary<string, string> ButtonCmd)
        {
            InlineKeyboardButton[][] keys = new InlineKeyboardButton[ButtonCmd.Count][];
            int i = 0;
            foreach (var button in ButtonCmd)
            {
                keys[i] = new InlineKeyboardButton[1] { button.Value };
                keys[i][0].CallbackData = button.Key;
                i++;
            }
            InlineKeyboardMarkup inkbd = new(keys);
            return inkbd;
        }
        static internal InlineKeyboardMarkup InKeyboard(Dictionary<string, string> ButtonCmd, string id)
        {
            InlineKeyboardButton[][] keys = new InlineKeyboardButton[ButtonCmd.Count][];
            int i = 0;
            foreach (var button in ButtonCmd)
            {
                keys[i] = new InlineKeyboardButton[1] { button.Value };
                if (button.Key.Equals("/cart", StringComparison.OrdinalIgnoreCase) || button.Key.Equals("/modcartitem", StringComparison.OrdinalIgnoreCase) 
                                                                                   || button.Key.Equals("/delcartitem", StringComparison.OrdinalIgnoreCase))
                {
                    keys[i][0].CallbackData =$"{button.Key} id*{id}";
                }
                else
                { 
                    keys[i][0].CallbackData = button.Key;
                }
                i++;
            }
            InlineKeyboardMarkup inkbd = new(keys);
            return inkbd;
        }

        static internal ReplyKeyboardMarkup MainKeyboard = new(new[]
        {
            new KeyboardButton[] {"View Cart", "Modify Cart", "Order"},
            new KeyboardButton[] {"Restart bot", "Upper menu"}
        })
        {
            ResizeKeyboard = true
        };

    }
}
