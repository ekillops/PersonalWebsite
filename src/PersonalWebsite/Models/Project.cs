using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
    public static class Project
    {
        public static void GetTopThreeStarred()
        {
            RestClient client = new RestClient("https://api.github.com/");
            RestRequest request = new RestRequest("users/ekillops/repos?access_token=" + EnvironmentVariables.GhApiToken);
            request.AddHeader("User-Agent", EnvironmentVariables.GhTokenName);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            Debug.WriteLine(response.Content);

            //var jsonResponse = JsonConvert.DeserializeObject<WeatherObservation>(response.Content);
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();

            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }

}
