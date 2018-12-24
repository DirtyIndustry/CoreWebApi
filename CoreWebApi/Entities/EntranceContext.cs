using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Entities
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
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedTokenConfiguration());
        }
    }
}
