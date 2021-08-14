using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using NetCoreMVC_GraphAPI_Integration.Helper;
using NetCoreMVC_GraphAPI_Integration.Models;
using NetCoreMVC_GraphAPI_Integration.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreMVC_GraphAPI_Integration.Controllers
{
    public class MeetingController : Controller
    {
        Dictionary<string, string> requestParameters;
        Dictionary<string, string> requestHeaderParameters;
        private readonly IOptions<Credentials> _credentials;
        private readonly IDistributedCache _distributedCache;
        private readonly IRefreshTokenService _refreshTokenService;
        public MeetingController(IOptions<Credentials> credentials, IDistributedCache distributedCache, IRefreshTokenService refreshTokenService)
        {
            _credentials = credentials;
            _distributedCache = distributedCache;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<IActionResult> GetAllMeeting()
        {
            var requestUrl = "https://graph.microsoft.com/v1.0/me/calendar/events";
            requestHeaderParameters = new();
            var token = await GetToken();

            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            requestHeaderParameters.Add("Authorization", tokens);
            var response = ApiRequestHelper.ApiRequest(null, requestHeaderParameters, requestUrl, Enums.HttpType.GET);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject eventList = JObject.Parse(response.Content);
                var calendarsEvent = eventList["value"].ToObject<IEnumerable<CalenderMeetingModel>>();
                return View(calendarsEvent);
            }

            return View();
        }

        public IActionResult CreateMeeting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(CalenderMeetingModel calenderMeetingModel)
        {
            var requestUrl = "https://graph.microsoft.com/v1.0/me/calendar/events";

            requestParameters = new();
            requestHeaderParameters = new();

            var token = await GetToken();

            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            if (token is not null)
            {
                requestHeaderParameters.Add("Authorization", tokens);
                requestHeaderParameters.Add("Content-Type", "application/json");
                requestParameters.Add("application/json", JsonConvert.SerializeObject(calenderMeetingModel));
                var response = ApiRequestHelper.ApiRequest(requestParameters, requestHeaderParameters, requestUrl, Enums.HttpType.POST);

                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return RedirectToAction("GetAllMeeting");
            }

            return RedirectToAction("GetAllMeeting");
        }

        public async Task<IActionResult> GetByEventId(string eventId)
        {
            var requestUrl = $"https://graph.microsoft.com/v1.0/me/calendar/events/{eventId}";
            requestHeaderParameters = new();

            var token = await _distributedCache.GetStringAsync("TokenFileContent");
            if (token is null)
                await _refreshTokenService.RefleshToken();

            var tokenValue = token.Split(',').ToList();

            requestHeaderParameters.Add("Authorization", "Bearer " + tokenValue[0].ToString());
            var response = ApiRequestHelper.ApiRequest(null, requestHeaderParameters, requestUrl, Enums.HttpType.GET);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject eventList = JObject.Parse(response.Content);
                var calendarsEvent = eventList["value"].ToObject<CalenderMeetingModel>();
                return View(calendarsEvent);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> UpdateMeeting(string eventId)
        {
            var requestUrl = $"https://graph.microsoft.com/v1.0/me/calendar/events/{eventId}";
            requestHeaderParameters = new();

            var token = await GetToken();
            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            requestHeaderParameters.Add("Authorization", tokens);
            requestHeaderParameters.Add("Prefer", "outlook.body-content-type=\"text\"");

            var response = ApiRequestHelper.ApiRequest(null, requestHeaderParameters, requestUrl, Enums.HttpType.GET);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var calendarsEvent = JObject.Parse(response.Content).ToObject<CalenderMeetingModel>();
                return View(calendarsEvent);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMeeting(string eventId, CalenderMeetingModel calenderEvent)
        {
            var requestUrl = $"https://graph.microsoft.com/v1.0/me/calendar/events/{eventId}";
            requestParameters = new();
            requestHeaderParameters = new();

            calenderEvent.End.TimeZone = "Etc/GMT";
            calenderEvent.Start.TimeZone = "Etc/GMT";

            var token = await GetToken();
            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            requestHeaderParameters.Add("Authorization", tokens);
            requestHeaderParameters.Add("Content-Type", "application/json");
            requestParameters.Add("application/json", JsonConvert.SerializeObject(calenderEvent));

            ApiRequestHelper.ApiRequest(requestParameters, requestHeaderParameters, requestUrl, Enums.HttpType.PATCH);

            return RedirectToAction("GetAllMeeting");
        }

        public async Task<ActionResult> DeleteMeeting(string eventId)
        {
            var requestUrl = $"https://graph.microsoft.com/v1.0/me/calendar/events/{eventId}";
            requestHeaderParameters = new();

            var token = await GetToken();
            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            requestHeaderParameters.Add("Authorization", tokens);

            ApiRequestHelper.ApiRequest(null, requestHeaderParameters, requestUrl, Enums.HttpType.DELETE);

            return RedirectToAction("GetAllMeeting");
        }

        public async Task<string> GetToken()
        {
            var token = await _distributedCache.GetStringAsync("TokenFileContent");

            if (token is null)
            {
                await _refreshTokenService.RefleshToken();
                token = await _distributedCache.GetStringAsync("TokenFileContent");
            }
            return token;
        }

        public IActionResult CreateOnlineMeeting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOnlineMeeting(CalenderOnlineMeetingModel calenderMeetingModel)
        {
            var requestUrl = "https://graph.microsoft.com/v1.0/me/onlineMeetings";

            requestParameters = new();
            requestHeaderParameters = new();

            var token = await GetToken();

            var tokenValue = token.Split(',')[2].ToString();
            var tokens = $"Bearer {tokenValue.Split(":")[1].Trim('"')}";

            requestHeaderParameters.Add("Authorization", tokens);
            requestHeaderParameters.Add("Content-Type", "application/json");
            requestParameters.Add("application/json", JsonConvert.SerializeObject(calenderMeetingModel));


            var response = ApiRequestHelper.ApiRequest(requestParameters, requestHeaderParameters, requestUrl, Enums.HttpType.POST);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var @event = JObject.Parse(response.Content);
                ViewBag.MeetAddress = @event["joinWebUrl"];
                return View();
            }

            return RedirectToAction("GetAllMeeting");
        }

    }
}
