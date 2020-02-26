using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FirstProject.DatabaseContext
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=MyDbConnection")
        {
        }

        public DbSet<ClientInfo> ClientInfos { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SupportProduct> SupportProducts { get; set; }
        public DbSet<ClientProduct> ClientProducts { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}