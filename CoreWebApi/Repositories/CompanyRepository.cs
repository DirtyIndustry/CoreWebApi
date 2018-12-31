﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly EntranceContext _entranceContext;
        public CompanyRepository(EntranceContext entranceContext)
        {
            _entranceContext = entranceContext;
        }

        public List<string> GetAllCompanies()
        {
            return _entranceContext.CompanyEntrances.Select(c => c.Name).ToList();
        }

        public bool CompanyExists(string name)
        {
            return _entranceContext.CompanyEntrances.Any(c => c.Name == name);
        }

        public void AddCompany(CompanyEntrance company)
        {
            _entranceContext.CompanyEntrances.Add(company);
        }
    }
}
