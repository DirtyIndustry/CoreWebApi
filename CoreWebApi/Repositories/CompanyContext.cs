using CoreWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public class CompanyContext: DbContext
    {
        private string database;
        public DbSet<User> Users { get; set; }

        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
            //Database.EnsureCreated();
            database = Database.GetDbConnection().Database;
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            #endregion

            #region Seed Data
            if (database == "DefaultCompany" | database == "")
            {
                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        Id = 1,
                        UserName = "root",
                        Department = "root",
                        Position = "root"
                    });
            }

            #endregion
        }
    }
}
