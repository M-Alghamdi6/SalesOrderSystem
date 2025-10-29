using Microsoft.EntityFrameworkCore;
using SalesOrderSystem_BackEnd.Models;
using System.Collections.Generic;

namespace SalesOrderSystem_BackEnd.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
     : base(options)
        {
        }

        public DbSet<UsersModel> Users { get; set; }
        public DbSet<SalesRequestModel> SalesRequest { get; set; }
        public DbSet<SalesRequestLineModel> SalesRequestLine { get; set; }


    }
}
