using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EntranceContext _entranceContext;
        public UnitOfWork(EntranceContext entranceContext)
        {
            _entranceContext = entranceContext;
        }

        /// <summary>
        /// Saves all changes to database.
        /// </summary>
        /// <returns>Save operation success or not</returns>
        public bool Save()
        {
            return _entranceContext.SaveChanges() >= 0;
        }
    }
}
