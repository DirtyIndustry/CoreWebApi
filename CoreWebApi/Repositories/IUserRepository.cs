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
        bool VerifyUser(string username, string password);
    }
}
