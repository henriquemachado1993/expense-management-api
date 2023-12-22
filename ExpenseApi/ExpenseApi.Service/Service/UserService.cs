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
            return ServiceResult<User>.CreateValidResult(await _repository.CreateAsync(user));
        }
        public async Task<ServiceResult<User>> UpdateAsync(User user, bool isUpdatePassword = true)
        {
            if(isUpdatePassword)
                user.Password = _passwordHasher.HashPassword(user.Password);

            return ServiceResult<User>.CreateValidResult(await _repository.UpdateAsync(user));
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
        }

        public async Task<ServiceResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression)
        {
            return ServiceResult<List<User>>.CreateValidResult(await _repository.FindAsync(filterExpression));
        }

        public async Task<ServiceResult<List<User>>> GetAllAsync()
        {
            return ServiceResult<List<User>>.CreateValidResult(await _repository.FindAsync(_ => true));
        }

        public async Task<ServiceResult<User>> GetByIdAsync(string id)
        {
            return ServiceResult<User>.CreateValidResult(await _repository.GetByIdAsync(new ObjectId(id)));
        }

        
    }
}
