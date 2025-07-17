using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connext.FunctionalTest.UIStepsDataManagement
{
    internal class Api
    {
        #region GET, POST, PUT, DELETE
        internal static IRestResponse GetObject(string url, string apiPath, string? apiKey = null)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(apiPath, Method.GET);
            request.AddHeader("API-KEY", "" + apiKey + "");
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse PostObject(string url, string apiPath, string? body = null, string? apiKey = null)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(apiPath, Method.POST);
            request.AddHeader("API-KEY", "" + apiKey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse PostObjectSessionID(string url, string apiPath, string? session_id = null, string? body = null, string? apiKey = null)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(apiPath, Method.POST);
            request.AddHeader("API-KEY", "" + apiKey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "session_id=" + session_id);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse PutObject(string url, string apiPath, string? body = null, string? apiKey = null)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(apiPath, Method.PUT);
            request.AddHeader("API-KEY", "" + apiKey + "");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            var response = Send(client, request);
            return response;
        }
        internal static IRestResponse DeleteObject(string url, string apiPath, string? msalIdToken = null)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(apiPath, Method.DELETE);
            request.AddHeader("x-access-token", "" + msalIdToken + "");
            var response = Send(client, request);
            return response;
        }
        #endregion

        #region Common
        internal static IRestResponse Send(RestClient? client, RestRequest? request)
        {
            IRestResponse response = client.Execute(request);
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var myException = new ApplicationException(message, response.ErrorException);
                throw myException;
            }
            return response;
        }
        #endregion
    }
}
