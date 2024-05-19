using BeireMKit.Domain.BaseModels;
using ExpenseApi.Controllers;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace ExpenseApi.Tests.Controller
{
    public class AuthControllerTest
    {
        private Mock<IAuthService> _authService;
        private AuthController _authController;

        private readonly string EMAIL = "admin@gmail.com";
        private readonly string PASSWORD = "12345";

        [SetUp]
        public void Setup()
        {
            _authService = new Mock<IAuthService>();
            _authController = new AuthController(_authService.Object);
        }

        [Test]
        public async Task AuthenticateIsValid()
        {
            // Arrange
            var login = new LoginRequestModel() { Email = EMAIL, Password = PASSWORD };
            _authService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(GetUser()));

            // Act
            var result = await _authController.Login(login);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<BaseResult<User>>(okResult?.Value);

            var serviceResult = okResult?.Value as BaseResult<User>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }

        [Test]
        public async Task AuthenticateIsInValid()
        {
            // Arrange
            var login = new LoginRequestModel() { Email = EMAIL, Password = PASSWORD };
            var resultAuthService = BaseResult<User>.CreateInvalidResult(message: "Não foi possível se autenticar.", statusCode:HttpStatusCode.Unauthorized);
            _authService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(resultAuthService));

            // Act
            var result = await _authController.Login(login);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);

            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult?.Value);
            Assert.IsInstanceOf<BaseResult<User>>(unauthorizedResult?.Value);

            var serviceResult = unauthorizedResult?.Value as BaseResult<User>;
            Assert.IsFalse(serviceResult?.IsValid);
            Assert.IsNull(serviceResult?.Data);
        }

        private static BaseResult<User> GetUser()
        {
            return BaseResult<User>.CreateValidResult(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                Email = "admin@gmail.com",
                BirthDate = DateTime.Now,
                Password = "12345",
                Address = new Address()
                {
                    City = "Brasilia",
                    Street = "X",
                    ZipCode = "123123"
                }
            });
        }
    }
}
