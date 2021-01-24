using KDSingleManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KDSingleManager
{
    public class AppContext : DbContext
    {
        public DbSet<Subcontractor> Subcontractors { get; set; }
        public DbSet<ZUS> Zusy { get; set; }
        //    public DbSet<Tax> Taxy { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = kdsingle.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
