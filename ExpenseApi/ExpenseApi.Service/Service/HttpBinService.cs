using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Interfaces;

namespace ExpenseApi.Service.Service
{
    /// <summary>
    /// Esse serviço tem a finalidade apenas de testar Resiliência em aplicações .Net com Polly
    /// </summary>
    public class HttpBinService : IHttpBinService
    {
        private readonly HttpClient _httpClient;
        public HttpBinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResult<object>> GetAsync(int code)
        {
            var response = await _httpClient.GetAsync($"http://httpbin.org/status/{code}");
            return BaseResult<object>.CreateValidResult(response.Content.ReadAsStringAsync());
        }
    }
}
