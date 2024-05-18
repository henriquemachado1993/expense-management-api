using BeireMKit.Authetication.Interfaces.Jwt;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using System.Net;
using System.Security.Claims;

namespace ExpenseApi.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _repository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IBaseRepository<User> repository, IPasswordHasherService passwordHasher, IJwtTokenService jwtTokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ServiceResult<User>> AuthenticateAsync(string email, string password)
        {
            email = email.Trim().ToLower();

            var resultUser = await _repository.FindAsync(x => x.Email.ToLower() == email);
            var user = resultUser?.FirstOrDefault();

            if (user != null && _passwordHasher.VerifyPassword(user.Password, password))
            {
                user.JwtToken = _jwtTokenService.GenerateJwtToken(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                });

                var result = await _repository.UpdateAsync(user);
                result.ClearPassword(clearToken: false);
                return ServiceResult<User>.CreateValidResult(result);
            }
            return ServiceResult<User>.CreateInvalidResult("Não foi possível se autenticar.", HttpStatusCode.Unauthorized);
        }
    }
}
