using System.Threading.Tasks;

namespace WPF_Client.Services
{
    public interface IHttpClient
    {
        Task<TResponse> GetAsync<TResponse>(string relativeUrl);

        void Post<TRequest, TResponse>(string relativeUrl, TRequest body);
    }
}
