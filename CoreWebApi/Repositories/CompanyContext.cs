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
        public DbSet<User> Users { get; set; }

        public CompanyContext(DbContextOptions<CompanyContext> options) : base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            #endregion

            #region Seed Data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Department = "headquarter",
                    Position = "CEO",
                    UserEntranceId = 1
                },
                new User
                {
                    Id = 2,
                    Department = "headquarter",
                    Position = "Boss",
                    UserEntranceId = 2
                },
                new User
                {
                    Id = 3,
                    Department = "headquarter",
                    Position = "Administrator",
                    UserEntranceId = 3
                });
            #endregion
        }
    }
}
