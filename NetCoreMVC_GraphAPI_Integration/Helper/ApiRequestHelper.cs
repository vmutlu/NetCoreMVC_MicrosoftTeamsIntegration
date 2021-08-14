using NetCoreMVC_GraphAPI_Integration.Enums;
using RestSharp;
using System;
using System.Collections.Generic;

namespace NetCoreMVC_GraphAPI_Integration.Helper
{
    public static class ApiRequestHelper
    {
        public static IRestResponse ApiRequest(Dictionary<string, string> requestParameters, Dictionary<string, string> requestHeaderParameters, string url, HttpType httpType)
        {
            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest();

            if (requestHeaderParameters != null)
                foreach (var item in requestHeaderParameters)
                    restRequest.AddHeader(item.Key, item.Value);

            if (requestParameters != null)
                foreach (var item in requestParameters)
                {
                    if (item.Key == "application/json")
                        restRequest.AddParameter(item.Key, item.Value, ParameterType.RequestBody);

                    else
                        restRequest.AddParameter(item.Key, item.Value);
                }

            restClient.BaseUrl = new Uri(url);
            dynamic response = default;

            switch (httpType)
            {
                case HttpType.GET:
                    response = restClient.Get(restRequest);
                    break;
                case HttpType.POST:
                    response = restClient.Post(restRequest);
                    break;
                case HttpType.PUT:
                    response = restClient.Put(restRequest);
                    break;
                case HttpType.PATCH:
                    response = restClient.Patch(restRequest);
                    break;
                case HttpType.DELETE:
                    response = restClient.Delete(restRequest);
                    break;
            }

            return response;
        }
    }
}
