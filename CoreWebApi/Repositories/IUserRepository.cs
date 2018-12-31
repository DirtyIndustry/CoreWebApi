using CoreWebApi.Entities;
using System.Collections.Generic;

namespace CoreWebApi.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Verifies user's validness by checking it's password.
        /// </summary>
        /// <param name="username">User's login identity</param>
        /// <param name="password">User's password</param>
        /// <returns>Validity of the user</returns>
        bool VerifyUser(UserEntrance user);

        UserEntrance GetUserInfo(string username);

        void AddUser(UserEntrance user);

        List<string> GetUserList();
    }
}
