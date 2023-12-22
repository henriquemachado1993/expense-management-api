﻿using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Models;

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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.AuthenticateAsync(model.Email, model.Password);

            if (!result.IsValid)
                return Unauthorized(result);

            // Pode retornar o token JWT ou outras informações do usuário
            return Ok(new { Token = result.Data.JwtToken });
        }
    }
}
