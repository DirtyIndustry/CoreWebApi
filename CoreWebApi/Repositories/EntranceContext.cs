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
        public DbSet<UserEntrance> UserEntrances { get; set; }
        public DbSet<CompanyEntrance> CompanyEntrances { get; set; }
        public DbSet<DeletedToken> DeletedTokens { get; set; }

        public EntranceContext(DbContextOptions<EntranceContext> options): base(options)
        {
            //Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations
            modelBuilder.ApplyConfiguration(new UserEntranceConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedTokenConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyEntranceConfiguration());
            #endregion

            #region SeedData
            modelBuilder.Entity<UserEntrance>().HasData(
                new UserEntrance
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin",
                    CompanyId = 1
                },
                new UserEntrance
                {
                    Id = 2,
                    UserName = "张三",
                    Password = "123",
                    CompanyId = 2
                },
                new UserEntrance
                {
                    Id = 3,
                    UserName = "李四",
                    Password = "123",
                    CompanyId = 3
                }
            );
            modelBuilder.Entity<CompanyEntrance>().HasData(
                new CompanyEntrance
                {
                    Id = 1,
                    Name = "Alibaba"
                },
                new CompanyEntrance
                {
                    Id = 2,
                    Name = "百度"
                },
                new CompanyEntrance
                {
                    Id = 3,
                    Name = "腾讯"
                });
            #endregion
        }
    }
}
