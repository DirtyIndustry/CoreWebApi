using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public interface IDeletedTokenRepository
    {
        /// <summary>
        /// Verifies a token's validness by checking it's existance in invalid-token table.
        /// </summary>
        /// <param name="jti">jti property of the token to verify</param>
        /// <returns>validity of the token</returns>
        bool VerifyToken(string jti);

        /// <summary>
        /// Marks a token invalid by adding it to an invalid-token table, used in user logout.
        /// </summary>
        /// <param name="deletedToken">Token that need to be marked invalid</param>
        void DeleteToken(DeletedToken deletedToken);

    }
}
