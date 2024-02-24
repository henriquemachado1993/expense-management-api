using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ServiceResult<object>> GetAsync(int code)
        {
            var response = await _httpClient.GetAsync($"http://httpbin.org/status/{code}");
            return ServiceResult<object>.CreateValidResult(response.Content.ReadAsStringAsync());
        }
    }
}
