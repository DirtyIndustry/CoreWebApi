using AutoMapper;
using CoreWebApi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoreWebApi.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IUnitOfWork<EntranceContext> unitOfWork;

        public LoginRepository(IUnitOfWork<EntranceContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Verify Login's UserName-Password pair.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool VerifyLogin(Login login)
        {
            return unitOfWork.DbContext.Logins.Any(x => x.UserName == login.UserName & x.Password == login.Password);
        }

        /// <summary>
        /// Check if a Login exists.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool LoginExists(string userName)
        {
            return unitOfWork.DbContext.Logins.Any(x => x.UserName == userName);
        }

        /// <summary>
        /// Check if a Company exists.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool CompanyExists(string company)
        {
            return unitOfWork.DbContext.Logins.Any(x => x.Company == company);
        }

        /// <summary>
        /// List All UserNames.
        /// </summary>
        /// <returns></returns>
        public List<string> GetLoginList()
        {
            return unitOfWork.DbContext.Logins.Select(x => x.UserName).ToList();
        }

        /// <summary>
        /// Get a Login's info, includes UserName, Password and Company.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Login GetLoginInfo(string userName)
        {
            return unitOfWork.DbContext.Logins.FirstOrDefault(l => l.UserName == userName);
        }

        public IEnumerable<Login> GetLoginsOfCompany(string company)
        {
            return unitOfWork.DbContext.Logins.Where(l => l.Company == company).ToList();
        }

        /// <summary>
        /// Register a new Login(User).
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public void AddLogin(Login login)
        {
            unitOfWork.DbContext.Logins.Add(login);
        }

        /// <summary>
        /// Delete a Login(User).
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public void RemoveLogin(Login login)
        {
            unitOfWork.DbContext.Logins.Remove(login);
        }

    }
}
