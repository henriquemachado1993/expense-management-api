using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using ExpenseApi.Service.Service;
using ExpenseApi.Tests.Helper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Tests.Service
{
    public class UserServiceTest
    {
        private Mock<IBaseRepository<User>> _repository;
        private Mock<IPasswordHasher> _passwordHasher;

        private UserService _service;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IBaseRepository<User>>();
            _passwordHasher = new Mock<IPasswordHasher>();

            _repository.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));
            _repository.Setup(x => x.UpdateAsync(It.IsAny<User>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));

            _passwordHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _passwordHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

            _service = new UserService(_repository.Object, _passwordHasher.Object);
        }

        [Test]
        public async Task InsertIsValid()
        {
            // Arrange
            var resultUser = new List<User>();
            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(resultUser));

            // Act
            var result = await _service.CreateAsync(UserModelsHelper.GetUser());

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task InsertUserExistsInvalid()
        {
            // Arrange
            var resultUser = new List<User>();
            resultUser.Add(UserModelsHelper.GetUser());
            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(resultUser));

            // Act
            var result = await _service.CreateAsync(UserModelsHelper.GetUser());

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task UpdateIsValid()
        {
            // Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));

            // Act
            var result = await _service.UpdateAsync(UserModelsHelper.GetUser());

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task InsertUserExists()
        {
            // Arrange
            User resultUser = default;
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(resultUser));

            // Act
            var result = await _service.UpdateAsync(UserModelsHelper.GetUser());

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task UpdatePasswordIsValid()
        {
            // Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));

            // Act
            var result = await _service.UpdatePasswordAsync(Guid.NewGuid(), "12345", "12345");

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task UpdatePasswordUserNotFound()
        {
            // Arrange
            User? resultUser = default;
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(resultUser));

            // Act
            var result = await _service.UpdatePasswordAsync(Guid.NewGuid(), "teste2", "teste2");

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task UpdatePasswordNotMatch()
        {
            // Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));
            _passwordHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _service.UpdatePasswordAsync(Guid.NewGuid(), "teste1", "teste2");

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task DeleteIsValid()
        {
            // Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));
            
            // Act
            var result = await _service.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task DeleteIsInvalid()
        {
            // Arrange
            User? resultUser = default;
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(resultUser));

            // Act
            var result = await _service.DeleteAsync(Guid.NewGuid());

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task FindIsValid()
        {
            // Arrange
            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(UserModelsHelper.GetListUserAsync()));

            // Act
            var result = await _service.FindAsync(x => true, true);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task GetAllIsValid()
        {
            // Arrange
            _repository.Setup(x => x.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).Returns(Task.FromResult(UserModelsHelper.GetListUserAsync()));

            // Act
            var result = await _service.GetAllAsync(true);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task GetByIdIsValid()
        {
            // Arrange
            _repository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(UserModelsHelper.GetUser()));

            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid(), true);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public async Task GetByIdIsinvalid()
        {
            // Arrange
            
            // Act
            var result = await _service.GetByIdAsync(Guid.NewGuid(), true);

            // Assert
            Assert.IsFalse(result.IsValid);
        }
    }
}