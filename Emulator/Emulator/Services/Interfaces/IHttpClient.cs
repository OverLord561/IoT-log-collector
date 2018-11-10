using System.Threading.Tasks;

namespace Emulator.Services.Interfaces
{
    public interface IHttpClient
    {
        Task<TResponse> Get<TResponse>(string relativeUrl);

        Task<TResponse> Post<TRequest, TResponse>(string relativeUrl, TRequest body);
    }
}
