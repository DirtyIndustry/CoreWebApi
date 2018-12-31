using Microsoft.Extensions.Caching.Distributed;

namespace CoreWebApi.Caching.Redis
{
    public interface IDistributedRedisCache: IDistributedCache
    {
        bool HasKey(string key);

        void UseDatabase(int databasenumber);
    }
}
