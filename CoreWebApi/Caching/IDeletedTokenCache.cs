using CoreWebApi.Entities;

namespace CoreWebApi.Caching
{
    public interface IDeletedTokenCache
    {
        void DeleteToken(DeletedToken deletedToken);
        bool VerifyToken(string jti);
    }
}