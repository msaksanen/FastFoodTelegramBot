using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFoodTelegramBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace FastFoodTelegramBot.Repositories
{
    
        public class ApplicationContext<T> : DbContext where T : BaseElement
        {
            public DbSet<T> SQL { get; set; }
           // public DbSet<T> SQL2 { get; set; }
           // public DbSet<T> SQL3{ get; set; }

        public string connectionString;
            public ApplicationContext(string connectionString)
            {
                this.connectionString = connectionString;   // получаем извне строку подключения

               // Database.EnsureDeleted();
                Database.EnsureCreated();
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<T>().HasKey(u => u.Id);
            }
        }
   
}
