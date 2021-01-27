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
        public DbSet<DefSkladki> DefinicjeSkladek { get; set; }
        public DbSet<Skladka> Skladki { get; set; }
        public DbSet<Przejscie> Przejscia { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = kdsingle.db");
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
