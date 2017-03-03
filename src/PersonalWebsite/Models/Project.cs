using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
    public static class Project
    {

        public static List<RepoQuery> GetTopThree()
        {
            RestClient client = new RestClient("https://api.github.com/");
            RestRequest request = new RestRequest("search/repositories?user=ekillops&sort=stars&order=desc&access_token=" + EnvironmentVariables.GhApiToken);
            request.AddHeader("User-Agent", EnvironmentVariables.GhTokenName);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            List<RepoQuery> topThree = JsonConvert.DeserializeObject<List<RepoQuery>>(response.Content);

            return topThree;
        }


        // OLD VERSION

        public static List<RepoQuery> GetTopThreeStarred()
        {
            RestClient client = new RestClient("https://api.github.com/");
            RestRequest request = new RestRequest("users/ekillops/repos?per_page=100&access_token=" + EnvironmentVariables.GhApiToken);
            request.AddHeader("User-Agent", EnvironmentVariables.GhTokenName);

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            List<RepoQuery> repos = JsonConvert.DeserializeObject<List<RepoQuery>>(response.Content);
            List<RepoQuery> topThree = TopThree(repos);
      
            return topThree;
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

        // Helper method to find top three starred repos
        private static List<RepoQuery> TopThree(List<RepoQuery> repos)
        {

            RepoQuery first = null;
            RepoQuery second = null;
            RepoQuery third = null;

            foreach(RepoQuery repo in repos)
            {
                Debug.WriteLine(repo.full_name + "****************************************");

                if (first == null)
                {
                    first = repo;
                }
                else if (repo.stargazers_count > first.stargazers_count)
                {
                    third = second;
                    second = first;
                    first = repo;
                }
                else if (second == null)
                {
                    second = repo;
                }
                else if (repo.stargazers_count == first.stargazers_count || repo.stargazers_count > second.stargazers_count)
                {
                    third = second;
                    second = repo;
                }
                else if (third == null)
                {
                    third = repo;
                }
                else if (repo.stargazers_count > third.stargazers_count)
                {
                    third = repo;
                }
            }

            List<RepoQuery> topThree = new List<RepoQuery>()
            {
                first, second, third
            };

            return topThree;
        }
    }

}
