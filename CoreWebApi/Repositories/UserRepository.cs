using CoreWebApi.Entities;
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

        public bool VerifyUser(User user)
        {
            return _entranceContext.Users.FirstOrDefault(u => u.UserName == user.UserName)?.Password == user.Password;
            // return _entranceContext.Users.Any(u => u.UserName == username & u.Password == password);
        }

        public User GetUserInfo(string username)
        {
            return _entranceContext.Users.FirstOrDefault(u => u.UserName == username);
        }

        public void AddUser(User user)
        {
            //if (_entranceContext.Users.Any(u => u.UserName == userDto.UserName))
            //{
            //    return false;
            //}
            _entranceContext.Users.Add(user);
        }
    }
}
