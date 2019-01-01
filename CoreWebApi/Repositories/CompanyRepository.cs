using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IUnitOfWork<EntranceContext> _unitOfWork;
        private readonly EntranceContext _entranceContext;

        public CompanyRepository(IUnitOfWork<EntranceContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entranceContext = unitOfWork.DbContext;
        }

        public List<string> GetAllCompanies()
        {
            return _entranceContext.CompanyEntrances.Select(c => c.CompanyName).ToList();
        }

        public bool CompanyExists(string name)
        {
            return _entranceContext.CompanyEntrances.Any(c => c.CompanyName == name);
        }

        public void AddCompany(CompanyEntrance company)
        {
            _entranceContext.CompanyEntrances.Add(company);
        }
    }
}
