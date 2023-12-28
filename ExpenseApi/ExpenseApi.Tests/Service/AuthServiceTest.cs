using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Service.Service;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace ExpenseApi.Tests.Service
{
    public class AuthServiceTest
    {
        private Mock<IBaseRepository<User>> _repository;
        private Mock<IPasswordHasher> _passwordHasher;
        private Mock<Microsoft.Extensions.Configuration.IConfiguration> _configuration;
        private Mock<JwtSecurityTokenHandler> _jwtSecurityTokenHandler;

        private AuthService _AuthService;

        private readonly string EMAIL = "admin@gmail.com";
        private readonly string PASSWORD = "12345";

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IBaseRepository<User>>();
            _passwordHasher = new Mock<IPasswordHasher>();
            
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

            _configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:SecretKey")]).Returns("ColoqueSuaChaveSecretaAquihahaha");
            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:Audience")]).Returns("Audience");
            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:Issuer")]).Returns("Issuer");

            _AuthService = new AuthService(_repository.Object, _passwordHasher.Object, _configuration.Object);
        }

        [Test]
        public async Task AuthenticateIsValid()
        {
            // Arrange
            var users = Task.FromResult(GetListUserAsync());
            var userCreate = Task.FromResult(GetUser());

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

        private List<User> GetListUserAsync()
        {
            var lst = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    BirthDate = DateTime.Now,
                    Password = "12345",
                    Address = new Address() {
                        City = "Brasilia",
                        Street = "X",
                        ZipCode = "123123"
                    }
                }
            };

            return lst;
        }

        private User GetUser()
        {
            var users = GetListUserAsync();
            return users.FirstOrDefault(x => x.Email == EMAIL) ?? new User();
        }
    }
}
