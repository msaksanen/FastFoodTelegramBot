using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FastFoodTelegramBot.Models
{
    public abstract class BaseElement
    {   
        [Key]
        public Guid Id { get; } = Guid.NewGuid();

    }
}
