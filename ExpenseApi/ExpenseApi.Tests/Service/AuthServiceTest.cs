using BeireMKit.Authetication.Interfaces.Jwt;
using BeireMKit.Data.Interfaces.MongoDB;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Service.Service;
using ExpenseApi.Tests.Helper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace ExpenseApi.Tests.Service
{
    public class AuthServiceTest
    {
        private Mock<IMongoRepository<User>> _repository;
        private Mock<IPasswordHasherService> _passwordHasher;
        private Mock<IJwtTokenService> _configuration;
        private Mock<JwtSecurityTokenHandler> _jwtSecurityTokenHandler;

        private AuthService _AuthService;

        private readonly string EMAIL = "admin@gmail.com";
        private readonly string PASSWORD = "12345";

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IMongoRepository<User>>();
            _passwordHasher = new Mock<IPasswordHasherService>();
            
            _jwtSecurityTokenHandler = new Mock<JwtSecurityTokenHandler>();
            _jwtSecurityTokenHandler.Setup(x => x.CreateToken(It.IsAny<SecurityTokenDescriptor>()))
                .Returns(new JwtSecurityToken(
                    "Issuer", 
                    "Audience", 
                    new List<Claim>() {
                        new Claim(ClaimTypes.Name, ""),
                    }, 
                    DateTime.Now, 
                    DateTime.Now.AddHours(5), 
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("ColoqueSuaChaveSecretaAquihahaha")), 
                    SecurityAlgorithms.HmacSha256Signature)
                ));
            _jwtSecurityTokenHandler.Setup(x => x.WriteToken(It.IsAny<SecurityToken>())).Returns("Token");

            _configuration = new Mock<IJwtTokenService>();
         
            _AuthService = new AuthService(_repository.Object, _passwordHasher.Object, _configuration.Object);
        }

        [Test]
        public async Task AuthenticateIsValid()
        {
            // Arrange
            var users = Task.FromResult(UserModelsHelper.GetListUserAsync());
            var userCreate = Task.FromResult(UserModelsHelper.GetUser());

            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(users);
            _passwordHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            _repository.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Returns(userCreate);

            // Act
            var result = await _AuthService.AuthenticateAsync(EMAIL, PASSWORD);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task AuthenticateIsInvalid()
        {
            // Arrange
            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult(new List<User>()));
            _passwordHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = await _AuthService.AuthenticateAsync(EMAIL, "123");

            // Assert
            Assert.IsTrue(!result.IsValid);
        }
    }
}
