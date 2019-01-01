using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public static class DbContextFactory
    {
        public static CompanyContext Create(string connString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyContext>();
            optionsBuilder.UseMySql(connString);
            return new CompanyContext(optionsBuilder.Options);
        }
    }
}
