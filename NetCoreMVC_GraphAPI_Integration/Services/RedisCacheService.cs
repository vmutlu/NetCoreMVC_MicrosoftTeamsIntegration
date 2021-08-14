using Microsoft.Extensions.Caching.Distributed;
using NetCoreMVC_GraphAPI_Integration.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;
        public RedisCacheService(IDistributedCache distributedCache) => _distributedCache = distributedCache;

        public async Task<TokenModel> AddRedisCache(TokenModel allData, int cacheTime, string cacheKey, bool force = false)
        {
            var dataNews = await _distributedCache.GetAsync(cacheKey);
            if (dataNews == null || force == true)
            {
                var data = JsonConvert.SerializeObject(allData);
                var dataByte = Encoding.UTF8.GetBytes(data);

                var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(cacheTime));
                option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheTime);
                await _distributedCache.SetAsync(cacheKey, dataByte, option);
            }

            var newsString = await _distributedCache.GetStringAsync(cacheKey);
            return JsonConvert.DeserializeObject<TokenModel>(newsString);
        }
    }
}
