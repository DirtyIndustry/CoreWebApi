using CoreWebApi.Caching.Redis;
using CoreWebApi.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApi.Caching
{
    public class DeletedTokenCache : IDeletedTokenCache
    {
        private readonly IDistributedRedisCache _distributedRedisCache;

        public DeletedTokenCache(IDistributedRedisCache distributedRedisCache)
        {
            _distributedRedisCache = distributedRedisCache;
        }

        public void DeleteToken(DeletedToken deletedToken)
        {
            _distributedRedisCache.SetString(deletedToken.Jti, value: deletedToken.Jti, options: new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = deletedToken.Exp
            });
        }

        public bool VerifyToken(string jti)
        {
            return !_distributedRedisCache.HasKey(jti);
        }
    }
}
