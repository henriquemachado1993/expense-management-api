using BeireMKit.Authetication.Interfaces.Jwt;
using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using System.Net;
using System.Security.Claims;

namespace ExpenseApi.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IMongoRepository<User> _repository;
        private readonly IPasswordHasherService _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(IMongoRepository<User> repository, IPasswordHasherService passwordHasher, IJwtTokenService jwtTokenService)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<BaseResult<User>> AuthenticateAsync(string email, string password)
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
                return BaseResult<User>.CreateValidResult(result);
            }
            return BaseResult<User>.CreateInvalidResult(message: $"Não foi possível se autenticar.", statusCode: HttpStatusCode.Unauthorized);
        }
    }
}
