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
            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = 1,
            //        UserName = "admin",
            //        Department = "headquarter",
            //        Position = "CEO"
            //    },
            //    new User
            //    {
            //        Id = 2,
            //        UserName = "张三",
            //        Department = "headquarter",
            //        Position = "Boss"
            //    },
            //    new User
            //    {
            //        Id = 3,
            //        UserName = "李四",
            //        Department = "headquarter",
            //        Position = "Administrator"
            //    });
            #endregion
        }
    }
}
