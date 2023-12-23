using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpenseApi.Domain.ValueObjects;

namespace ExpenseApi.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _repository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IBaseRepository<User> repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult<User>> CreateAsync(User user)
        {
            user.Id = ObjectId.GenerateNewId();
            user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.CreateAsync(user);
            entity.ClearPassword();
            return ServiceResult<User>.CreateValidResult();
        }
        public async Task<ServiceResult<User>> UpdateAsync(User user, bool isUpdatePassword = true)
        {
            if(isUpdatePassword)
                user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.UpdateAsync(user);
            entity.ClearPassword();
            return ServiceResult<User>.CreateValidResult(entity);
        }

        public async Task<ServiceResult<User>> UpdatePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var entity = await _repository.GetByIdAsync(new ObjectId(userId));
            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            if (!_passwordHasher.VerifyPassword(entity.Password, oldPassword))
                return ServiceResult<User>.CreateInvalidResult("A senha anterior está incorreta.");

            entity.Password = _passwordHasher.HashPassword(newPassword);

            var result = await _repository.UpdateAsync(entity);

            result.ClearPassword();

            return ServiceResult<User>.CreateValidResult(result);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
        }

        public async Task<ServiceResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression)
        {
            var result = await _repository.FindAsync(filterExpression);

            result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult();
        }

        public async Task<ServiceResult<List<User>>> GetAllAsync()
        {
            var result = await _repository.FindAsync(_ => true);

            result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult();
        }

        public async Task<ServiceResult<User>> GetByIdAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(new ObjectId(id));
            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            entity.ClearPassword();

            return ServiceResult<User>.CreateValidResult(entity);
        }

        
    }
}
