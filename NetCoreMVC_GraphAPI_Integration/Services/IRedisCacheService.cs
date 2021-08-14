using NetCoreMVC_GraphAPI_Integration.Models;
using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Services
{
    public interface IRedisCacheService
    {
        Task<TokenModel> AddRedisCache(TokenModel allData, int cacheTime, string cacheKey, bool force = false);
    }
}
