using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves all changes to database.
        /// </summary>
        /// <returns>Save operation success or not</returns>
        bool Save();
    }
}
