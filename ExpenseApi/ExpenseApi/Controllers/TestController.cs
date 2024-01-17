using Microsoft.AspNetCore.Mvc;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para autenticação do usuário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        /// <summary>
        /// Faz login pelo Email e senha
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(new List<int> { 1, 2, 3 });
        }
    }
}
