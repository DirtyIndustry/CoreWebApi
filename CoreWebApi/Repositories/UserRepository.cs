using CoreWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUnitOfWork<CompanyContext> unitOfWork;
        public IUnitOfWork<CompanyContext> UnitOfWork => unitOfWork;

        public UserRepository(IUnitOfWork<CompanyContext> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public User GetUserInfo(string userName)
        {
            return unitOfWork.DbContext.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public List<string> GetUserList()
        {
            return unitOfWork.DbContext.Users.Select(u => u.UserName).ToList();
        }

        public bool UserExists(string userName)
        {
            return unitOfWork.DbContext.Users.Any(u => u.UserName == userName);
        }

        public IEnumerable<User> GetUsersOfDepartment(string department)
        {
            return unitOfWork.DbContext.Users.Where(u => u.Department == department).ToList();
        }

        public IEnumerable<User> GetUsersOfPosition(string position)
        {
            return unitOfWork.DbContext.Users.Where(u => u.Position == position).ToList();
        }

        public void AddUser(User user)
        {
            unitOfWork.DbContext.Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            unitOfWork.DbContext.Users.Remove(user);
        }
    }
}
