using Emulator.Config.Interfaces;
using Emulator.Services.Interfaces;
using RestSharp;
using System.Threading.Tasks;

namespace Emulator.Services
{
    public class RestSharpHttpClient : IHttpClient
    {
        private readonly IEmulatorConfiguration _configuration;
        private readonly string _baseUrl;

        public RestSharpHttpClient(IEmulatorConfiguration configuration)
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
