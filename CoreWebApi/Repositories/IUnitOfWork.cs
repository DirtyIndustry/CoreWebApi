using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public interface IUnitOfWork<TContext> where TContext: DbContext
    {
        /// <summary>
        /// Get the DbContext.
        /// </summary>
        TContext DbContext { get; }

        void ChangeDatabase(string database);

        /// <summary>
        /// Saves all changes to database.
        /// </summary>
        /// <returns>Save operation success or not</returns>
        bool Save();
    }
}
