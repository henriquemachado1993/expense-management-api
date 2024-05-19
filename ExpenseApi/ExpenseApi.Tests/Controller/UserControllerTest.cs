using AutoMapper;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Controllers;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Tests.Helper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

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
            var users = BaseResult<List<User>>.CreateValidResult(UserModelsHelper.GetListUserAsync());
            _userService.Setup(x => x.GetAllAsync(It.IsAny<bool>())).Returns(Task.FromResult(users));

            // Act
            var result = await _userController.Get();

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<BaseResult<List<User>>>(okResult?.Value);

            var serviceResult = okResult?.Value as BaseResult<List<User>>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }

        [Test]
        public async Task GetByIdIsValid()
        {
            // Arrange
            var user = BaseResult<User>.CreateValidResult(UserModelsHelper.GetUser());
            _userService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(Task.FromResult(user));

            // Act
            var result = await _userController.Get(Guid.NewGuid());

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
        public async Task GetByNameIsValid()
        {
            // Arrange
            var user = BaseResult<List<User>>.CreateValidResult(UserModelsHelper.GetListUserAsync());
            _userService.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).Returns(Task.FromResult(user));

            // Act
            var result = await _userController.GetByName("teste");

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            Assert.IsInstanceOf<BaseResult<List<User>>>(okResult?.Value);

            var serviceResult = okResult?.Value as BaseResult<List<User>>;
            Assert.IsTrue(serviceResult?.IsValid);
            Assert.IsNotNull(serviceResult?.Data);
        }
    }
}
