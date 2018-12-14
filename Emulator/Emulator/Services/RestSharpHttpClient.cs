using Emulator.Config.Interfaces;
using Emulator.Services.Interfaces;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Emulator.Services
{
    public class RestSharpHttpClient
    {
        //private readonly IEmulatorConfiguration _configuration;
        //private readonly string _baseUrl;

        private static readonly RestClient _client = new RestClient("http://localhost:5000");

        public RestSharpHttpClient(IEmulatorConfiguration configuration)
        {
            //_configuration = configuration;
            //_baseUrl = _configuration.GetServerSettings().BaseUrl;
        }

        public Task<TResponse> Get<TResponse>(string relativeUrl)
        {
            throw new NotImplementedException();
        }

        //public async Task<TResponse> Get<TResponse>(string relativeUrl)
        //{
        //    var client = new RestClient(_baseUrl);
        //    var request = new RestRequest(relativeUrl, Method.GET);

        //    IRestResponse<TResponse> res = await client.ExecuteTaskAsync<TResponse>(request);

        //    return res.Data;
        //}

        public void Post<TRequest, TResponse>(string relativeUrl, TRequest body, RestClient client)
        {
            //var baseUrl = _configuration.GetServerSettings().BaseUrl;

            //var client = new RestClient(baseUrl);
            //var client = _client;

            var request = new RestRequest(relativeUrl, Method.POST) { RequestFormat = RestSharp.DataFormat.Json };

            request.AddBody(body);

             client.Execute(request);
            
        }
    }
}
