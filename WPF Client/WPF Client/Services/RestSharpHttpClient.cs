﻿using RestSharp;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace WPF_Client.Services
{
    public class RestSharpHttpClient: IHttpClient
    {
        private static readonly RestClient _client;       

        static RestSharpHttpClient()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["BaseUrl"]);
        }

        public async Task<TResponse> GetAsync<TResponse>(string relativeUrl)
        {
            var request = new RestRequest(relativeUrl, Method.GET);

            IRestResponse<TResponse> res = await _client.ExecuteTaskAsync<TResponse>(request);

            return JsonConvert.DeserializeObject<TResponse>(res.Content);
        }

        public void Post<TRequest, TResponse>(string relativeUrl, TRequest body)
        {
            throw new NotImplementedException();
        }
    }
}
