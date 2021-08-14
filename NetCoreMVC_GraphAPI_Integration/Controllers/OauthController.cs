using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NetCoreMVC_GraphAPI_Integration.Enums;
using NetCoreMVC_GraphAPI_Integration.Helper;
using NetCoreMVC_GraphAPI_Integration.Models;
using NetCoreMVC_GraphAPI_Integration.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Controllers
{
    public class OauthController : Controller
    {
        private readonly IOptions<Credentials> _credentials;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IDistributedCache _distributedCache;
        private readonly IRefreshTokenService _refreshTokenService;

        Dictionary<string, string> requestParameters;
        Dictionary<string, string> requestHeaderParameters;
        public OauthController(IOptions<Credentials> credentials, IDistributedCache distributedCache, IRedisCacheService redisCacheService, IRefreshTokenService refreshTokenService)
        {
            _credentials = credentials;
            _distributedCache = distributedCache;
            _redisCacheService = redisCacheService;
            _refreshTokenService = refreshTokenService;
        }

        public ActionResult Login()
        {
            var redirectUrl = $"https://login.microsoftonline.com/{_credentials.Value.TenantId}/oauth2/v2.0/authorize?" +
                "&scope=" + _credentials.Value.Scopes +
                "&response_type=code" +
                "&response_mode=query" +
                "&state=themessydeveloper" +
                "&redirect_uri=" + _credentials.Value.RedirectUrl +
                "&client_id=" + _credentials.Value.ClientId;

            return Redirect(redirectUrl);
        }

        public ActionResult AdminLogin()
        {
            var redirectUrl = "https://login.microsoftonline.com/common/adminconsent?" +
                "&state=themessydeveloper" +
                "&redirect_uri=" + _credentials.Value.RedirectUrl +
                "&client_id=" + _credentials.Value.ClientId;

            return Redirect(redirectUrl);
        }

        public async Task<ActionResult> Callback(string code)
        {
            string requestUrl = $"https://login.microsoftonline.com/{_credentials.Value.TenantId}/oauth2/v2.0/token";
            requestParameters = new();
            requestHeaderParameters = new();

            if (!string.IsNullOrWhiteSpace(code))
            {
                requestHeaderParameters.Add("Content-Type", "application/x-www-form-urlencoded");
                requestParameters.Add("client_id", _credentials.Value.ClientId);
                requestParameters.Add("scope", _credentials.Value.Scopes);
                requestParameters.Add("redirect_uri", _credentials.Value.RedirectUrl);
                requestParameters.Add("code", code);
                requestParameters.Add("grant_type", "authorization_code");
                requestParameters.Add("client_secret", _credentials.Value.ClientSecret);

                var response = ApiRequestHelper.ApiRequest(requestParameters, requestHeaderParameters, requestUrl, httpType: HttpType.POST);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject token = JObject.Parse(response.Content);
                    var tokenModels = token.ToObject<TokenModel>();

                    System.IO.File.WriteAllText("refreshToken.json", response.Content);
                    await _distributedCache.RemoveAsync("TokenFileContent");
                    await _redisCacheService.AddRedisCache(tokenModels, 30, "TokenFileContent", true);
                }
            }

            return RedirectToAction("CreateMeeting", "Meeting");
        }

        public async Task<ActionResult> RefleshToken()
        {
            await _refreshTokenService.RefleshToken();
            return RedirectToAction("CreateMeeting", "Meeting");
        }
    }
}
