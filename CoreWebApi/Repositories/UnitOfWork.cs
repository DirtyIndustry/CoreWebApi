using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
    {
        private TContext _context;
        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public TContext DbContext => _context;

        public void ChangeDatabase(string database)
        {
            var connection = _context.Database.GetDbConnection();
            if (connection.State.HasFlag(ConnectionState.Open))
            {
                connection.ChangeDatabase(database);
            }
            else
            {
                var connectionString = @"Server=127.0.0.1;database=defaultdb;uid=myuser;pwd=mypass";
                connectionString = Regex.Replace(connectionString.Replace(" ", ""), @"(?<=[Dd]atabase=)\w+(?=;)", database, RegexOptions.Singleline);
                connection.ConnectionString = connectionString;
                _context = DbContextFactory.Create(connectionString) as TContext;
            }
            var items = _context.Model.GetEntityTypes();
            foreach(var item in items)
            {
                if (item.Relational() is RelationalEntityTypeAnnotations extensions)
                {
                    extensions.Schema = database;
                }
            }
        }

        /// <summary>
        /// Saves all changes to database.
        /// </summary>
        /// <returns>Save operation success or not</returns>
        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
