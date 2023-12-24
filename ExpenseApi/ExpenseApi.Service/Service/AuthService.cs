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
        private readonly IBaseRepository<User> _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthService(IBaseRepository<User> repository, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public async Task<ServiceResult<User>> AuthenticateAsync(string email, string password)
        {
            email = email.Trim().ToLower();

            var resultUser = await _repository.FindAsync(x => x.Email.ToLower() == email);
            var user = resultUser?.FirstOrDefault();

            if (user != null && _passwordHasher.VerifyPassword(user.Password, password))
            {
                user.JwtToken = GenerateJwtToken(user);

                var result = await _repository.UpdateAsync(user);
                result.ClearPassword();
                return ServiceResult<User>.CreateValidResult(result);
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
