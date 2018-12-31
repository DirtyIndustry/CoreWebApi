using CoreWebApi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoreWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EntranceContext _entranceContext;
        public UserRepository(EntranceContext entranceContext)
        {
            _entranceContext = entranceContext;
        }

        public bool VerifyUser(UserEntrance user)
        {
            return _entranceContext.UserEntrances.FirstOrDefault(u => u.UserName == user.UserName)?.Password == user.Password;
            // return _entranceContext.Users.Any(u => u.UserName == username & u.Password == password);
        }

        public UserEntrance GetUserInfo(string username)
        {
            return _entranceContext.UserEntrances.FirstOrDefault(u => u.UserName == username);
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
