using System.Linq;
using CoreWebApi.Entities;

namespace CoreWebApi.Repositories
{
    public class DeletedTokenRepository : IDeletedTokenRepository
    {
        private readonly EntranceContext _entranceContext;
        public DeletedTokenRepository(EntranceContext entranceContext)
        {
            _entranceContext = entranceContext;
        }

        /// <summary>
        /// Marks a token invalid by adding it to an invalid-token table, used in user logout.
        /// </summary>
        /// <param name="deletedToken">Token that need to be marked invalid</param>
        public void DeleteToken(DeletedToken deletedToken)
        {
            _entranceContext.DeletedTokens.Add(deletedToken);
        }


        /// <summary>
        /// Verifies a token's validness by checking it's existance in invalid-token table.
        /// </summary>
        /// <param name="jti">jti property of the token to verify</param>
        /// <returns>validity of the token</returns>
        public bool VerifyToken(string jti)
        {
            return !_entranceContext.DeletedTokens.Any(o => o.Jti == jti);
        }
    }
}
