using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NetCoreMVC_GraphAPI_Integration.Enums;
using NetCoreMVC_GraphAPI_Integration.Helper;
using NetCoreMVC_GraphAPI_Integration.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IOptions<Credentials> _credentials;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IDistributedCache _distributedCache;
        Dictionary<string, string> requestParameters;
        public RefreshTokenService(IOptions<Credentials> credentials, IDistributedCache distributedCache, IRedisCacheService redisCacheService)
        {
            _credentials = credentials;
            _distributedCache = distributedCache;
            _redisCacheService = redisCacheService;
        }

        public async Task RefleshToken()
        {
            JObject refreshToken = JObject.Parse(System.IO.File.ReadAllText("refreshToken.json"));
            var refreshValue = refreshToken.ToObject<TokenModel>();

            string requestUrl = $"https://login.microsoftonline.com/{_credentials.Value.TenantId}/oauth2/v2.0/token";
            requestParameters = new();

            requestParameters.Add("client_id", _credentials.Value.ClientId);
            requestParameters.Add("grant_type", "refresh_token");
            requestParameters.Add("scope", _credentials.Value.Scopes);
            requestParameters.Add("refresh_token", refreshValue.refresh_token);
            requestParameters.Add("redirect_uri", _credentials.Value.RedirectUrl);
            requestParameters.Add("client_secret", _credentials.Value.ClientSecret);

            var response = ApiRequestHelper.ApiRequest(requestParameters, null, requestUrl, HttpType.POST);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject token = JObject.Parse(response.Content);
                var tokenModels = token.ToObject<TokenModel>();

                System.IO.File.WriteAllText("refreshToken.json", response.Content);
                await _distributedCache.RemoveAsync("TokenFileContent");
                await _redisCacheService.AddRedisCache(tokenModels, 30, "TokenFileContent", true);
            }
        }
    }
}
