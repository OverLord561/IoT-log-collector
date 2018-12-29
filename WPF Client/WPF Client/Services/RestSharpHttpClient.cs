using RestSharp;
using System;
using System.Threading.Tasks;

namespace WPF_Client.Services
{
    public class RestSharpHttpClient: IHttpClient
    {
        private static readonly RestClient _client;       

        static RestSharpHttpClient()
        {
            _client = new RestClient("https://localhost:44373");
        }

        public async Task<TResponse> GetAsync<TResponse>(string relativeUrl)
        {
            var request = new RestRequest(relativeUrl, Method.GET);

            IRestResponse<TResponse> res = await _client.ExecuteTaskAsync<TResponse>(request);

            return res.Data;
        }

        public void Post<TRequest, TResponse>(string relativeUrl, TRequest body)
        {
            throw new NotImplementedException();
        }
    }
}
