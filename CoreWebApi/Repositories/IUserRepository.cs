using System.Collections.Generic;
using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUserInfo(string userName);
        List<string> GetUserList();
        IEnumerable<User> GetUsersOfDepartment(string department);
        IEnumerable<User> GetUsersOfPosition(string position);
        void RemoveUser(User user);
        bool UserExists(string userName);
    }
}