using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// Essa controller tem finalidade exclusiva de testar Resiliência em aplicações .Net com Polly
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PollyTestsController : Controller
    {
        private readonly IHttpBinService _service;

        public PollyTestsController(IHttpBinService service)
        {
            _service = service;
        }

        /// <summary>
        /// Faz um teste simples com polly
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{code:int}")]
        public async Task<ServiceResult<object>> Get([FromRoute] int code)
        {
            return await _service.GetAsync(code);
        }
    }
}
