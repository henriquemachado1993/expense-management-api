using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ExpenseApi.Domain.ValueObjects;
using System.Net;

namespace ExpenseApi.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IUserService userService, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<ServiceResult<User>> AuthenticateAsync(string email, string password)
        {
            var result = await _userService.FindAsync(x => x.Email.ToLower() == email.ToLower(), false);
            var user = result.Data?.FirstOrDefault();

            if (user != null && _passwordHasher.VerifyPassword(user.Password, password))
            {
                user.JwtToken = GenerateJwtToken(user);
                await _userService.UpdateAsync(user, false);
                return ServiceResult<User>.CreateValidResult(user);
            }
            return ServiceResult<User>.CreateInvalidResult("Não foi possível se autenticar.", HttpStatusCode.Unauthorized);
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var audience = _configuration["JwtSettings:Audience"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = audience,
                Issuer = issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
