using Emulator.Config.Interfaces;
using Emulator.Models;
using Emulator.Services.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Emulator.Services
{
    public class HttpClient : IHttpClient
    {
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public HttpClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _baseUrl = _configuration.GetServerSettings().BaseUrl;
        }

        public async Task<TResponse> Get<TResponse>(string relativeUrl)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest(relativeUrl, Method.GET);

            IRestResponse<TResponse> res = await client.ExecuteTaskAsync<TResponse>(request);

            return res.Data;
        }

        public async Task<TResponse> Post<TRequest, TResponse>(string relativeUrl, TRequest body )
        {
            var baseUrl = _configuration.GetServerSettings().BaseUrl;
            var client = new RestClient(baseUrl);
            var request = new RestRequest(relativeUrl, Method.POST) { RequestFormat = RestSharp.DataFormat.Json };

            request.AddBody(body);

            IRestResponse<TResponse> res = await client.ExecuteTaskAsync<TResponse>(request);

            return res.Data;
        }
    }
}
