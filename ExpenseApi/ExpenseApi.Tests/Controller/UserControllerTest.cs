using AutoMapper;
using ExpenseApi.Controllers;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Models.Auth;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Service.Service;
using ExpenseApi.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Tests.Controller
{
    public class UserControllerTest
    {
        private Mock<IUserService> _userService;
        private Mock<IMapper> _mapper;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
            _mapper = new Mock<IMapper>();
            _userController = new UserController(_userService.Object, _mapper.Object);
        }

        [Test]
        public async Task GetAllIsValid()
        {
            // Arrange
            var users = ServiceResult<List<User>>.CreateValidResult(UserModelsHelper.GetListUserAsync());
            _userService.Setup(x => x.GetAllAsync(It.IsAny<bool>())).Returns(Task.FromResult(users));

            // Act
            var result = await _userController.Get();

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<ServiceResult<List<User>>>(okResult?.Value);

            var serviceResult = okResult?.Value as ServiceResult<List<User>>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }

        [Test]
        public async Task GetByIdIsValid()
        {
            // Arrange
            var user = ServiceResult<User>.CreateValidResult(UserModelsHelper.GetUser());
            _userService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(user));

            // Act
            var result = await _userController.Get(Guid.NewGuid());

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<ServiceResult<User>>(okResult?.Value);

            var serviceResult = okResult?.Value as ServiceResult<User>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }

        [Test]
        public async Task GetByNameIsValid()
        {
            // Arrange
            var user = ServiceResult<List<User>>.CreateValidResult(UserModelsHelper.GetListUserAsync());
            _userService.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(Task.FromResult(user));

            // Act
            var result = await _userController.GetByName("teste");

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<ServiceResult<List<User>>>(okResult?.Value);

            var serviceResult = okResult?.Value as ServiceResult<List<User>>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }
    }
}
