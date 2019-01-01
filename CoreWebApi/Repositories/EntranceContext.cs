using CoreWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public class EntranceContext: DbContext
    {
        public DbSet<Login> Logins { get; set; }
        public DbSet<DeletedToken> DeletedTokens { get; set; }

        public EntranceContext(DbContextOptions<EntranceContext> options): base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations
            modelBuilder.ApplyConfiguration(new LoginConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedTokenConfiguration());
            #endregion

            #region SeedData
            modelBuilder.Entity<Login>().HasData(
                new Login
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin",
                    Company = "DefaultCompany"
                },
                new Login
                {
                    Id = 2,
                    UserName = "张三",
                    Password = "123",
                    Company = "DefaultCompany"
                },
                new Login
                {
                    Id = 3,
                    UserName = "李四",
                    Password = "123",
                    Company = "DefaultCompany"
                }
            );
            #endregion
        }
    }
}
