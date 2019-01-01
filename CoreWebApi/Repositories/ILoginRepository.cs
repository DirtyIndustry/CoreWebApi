using System.Collections.Generic;
using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public interface ILoginRepository
    {
        void AddLogin(Login login);
        Login GetLoginInfo(string userName);
        List<string> GetLoginList();
        IEnumerable<Login> GetLoginsOfCompany(string company);
        bool LoginExists(string userName);
        bool CompanyExists(string company);
        void RemoveLogin(Login login);
        bool VerifyLogin(Login login);
    }
}