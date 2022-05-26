using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodTelegramBot.Models
{
    class LoggerFailureException : Exception
    {
        public LoggerFailureException() { }
        public LoggerFailureException(string message) : base(message) { }

    }
}
