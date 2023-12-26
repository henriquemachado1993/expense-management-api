using Castle.Core.Configuration;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.ValueObjects;
using ExpenseApi.Service.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Tests.Service
{
    [TestFixture]
    public class AuthServiceTest
    {
        private readonly Mock<IBaseRepository<User>> _repository;
        private readonly Mock<IPasswordHasher> _passwordHasher;
        private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _configuration;

        private readonly string EMAIL = "admin@gmail.com";
        private readonly string PASSWORD = "12345";

        public AuthServiceTest()
        {
            _repository = new Mock<IBaseRepository<User>>();
            _passwordHasher = new Mock<IPasswordHasher>();
            _configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        }

        [Test]
        public void Setup()
        {
            var users = GetListUserAsync();
            var userCreate = GetUser();

            _repository.Setup(x => x.FindAsync(x => x.Email.ToLower() == EMAIL))
                .Returns(Task.FromResult(users));

            _repository.Setup(x => x.UpdateAsync(userCreate))
                .Returns(Task.FromResult(userCreate));

            //var mockSection = new Mock<Microsoft.Extensions.Configuration.IConfigurationSection>();
            //mockSection.Setup(x => x.Value).Returns("SecretKey");
            
            //var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            //mockConfig.Setup(x => x.GetSection("ConfigKey")).Returns(mockSection.Object);

            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:SecretKey")]).Returns("SecretKey");
            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:Audience")]).Returns("Audience");
            _configuration.Setup(x => x[It.Is<string>(s => s == "JwtSettings:Issuer")]).Returns("Issuer");
            
        }

        [Test]
        public async Task AuthenticateValid()
        {
            // Arrange

            Setup();

            _passwordHasher.Setup(x => x.VerifyPassword("12345", "12345"))
               .Returns(true);

            // Act
            var result = await new AuthService(_repository.Object, _passwordHasher.Object, _configuration.Object).AuthenticateAsync(EMAIL, PASSWORD);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task AuthenticateInvalid()
        {
            // Arrange
           
            // Act
            //var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            //Assert.IsFalse(!result.IsValid);
        }

        [Test]
        public async Task AuthenticateVerifyIfUserNull()
        {
            // Arrange
            var email = "3213213123123123123123123123123123@gmail.com";
            var password = "12345";

            // Act
            //var result = await _authService.AuthenticateAsync(email, password);

            // Assert
            //Assert.IsFalse(!result.IsValid);
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
            return users.FirstOrDefault(x => x.Email == EMAIL);
        }
    }
}
