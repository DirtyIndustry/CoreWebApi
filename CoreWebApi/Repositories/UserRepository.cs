using CoreWebApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CoreWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EntranceContext _entranceContext;
        private readonly IUnitOfWork<EntranceContext> _unitOfWork;
        private readonly IUnitOfWork<CompanyContext> _companyUnitOfWork;

        public UserRepository(IUnitOfWork<EntranceContext> unitOfWork, IUnitOfWork<CompanyContext> companyUnitOfWork)
        {
            _entranceContext = unitOfWork.DbContext;
            _unitOfWork = unitOfWork;
            _companyUnitOfWork = companyUnitOfWork;
        }

        public bool VerifyUser(UserEntrance user)
        {
            return _entranceContext.UserEntrances.FirstOrDefault(u => u.UserName == user.UserName)?.Password == user.Password;
            // return _entranceContext.Users.Any(u => u.UserName == username & u.Password == password);
        }

        public UserEntrance GetUserInfo(string username)
        {
            var userentrance = _entranceContext.UserEntrances.FirstOrDefault(u => u.UserName == username);
            if (userentrance == null)
            {
                return null;
            }
            var company = _entranceContext.CompanyEntrances.FirstOrDefault(c => c.Id == userentrance.CompanyId);
            var result = _entranceContext.UserEntrances.Include(u => u.Company).Include(u => u.User).FirstOrDefault(u => u.UserName == username);


            System.Diagnostics.Debug.WriteLine(result);
            return null;
        }

        public void AddUser(UserEntrance user)
        {
            //if (_entranceContext.Users.Any(u => u.UserName == userDto.UserName))
            //{
            //    return false;
            //}
            _entranceContext.UserEntrances.Add(user);
        }

        public List<string> GetUserList()
        {
            return _entranceContext.UserEntrances.Select(u => u.UserName).ToList();
        }
    }
}
