using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWebAPI.Services.Interfaices
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsynk(string cahedKey, object response, TimeSpan timeLive);

        Task<string> GetCacheResponseAsynk(string cacheKey);
    }
}
