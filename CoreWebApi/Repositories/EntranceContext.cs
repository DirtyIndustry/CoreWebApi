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
        public DbSet<User> Users { get; set; }
        public DbSet<DeletedToken> DeletedTokens { get; set; }

        public EntranceContext(DbContextOptions<EntranceContext> options): base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedTokenConfiguration());
            #endregion

            #region SeedData
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin"
                },
                new User
                {
                    Id = 2,
                    UserName = "张三",
                    Password = "123"
                },
                new User
                {
                    Id = 3,
                    UserName = "李四",
                    Password = "123"
                }
            );
            #endregion
        }
    }
}
