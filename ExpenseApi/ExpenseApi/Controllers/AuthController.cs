using Microsoft.AspNetCore.Mvc;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Helper;
using ExpenseApi.Domain.Models.Auth;

namespace ExpenseApi.Controllers
{
    /// <summary>
    /// API para autenticação do usuário.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// API para autenticação do usuário.
        /// </summary>
        /// <param name="authService"></param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Faz login pelo Email e senha
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var result = await _authService.AuthenticateAsync(model.Email, model.Password);

            return ResponseHelper.Handle(result);
        }
    }
}
