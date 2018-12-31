using CoreWebApi.Entities;
using System.Collections.Generic;

namespace CoreWebApi.Repositories
{
    public interface ICompanyRepository
    {
        List<string> GetAllCompanies();
        bool CompanyExists(string name);
        void AddCompany(CompanyEntrance company);
    }
}
