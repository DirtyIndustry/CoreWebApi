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

        public bool VerifyUser(string username, string password)
        {
            return _entranceContext.Users.FirstOrDefault(u => u.UserName == username)?.Password == password;
            // return _entranceContext.Users.Any(u => u.UserName == username & u.Password == password);
        }
    }
}
