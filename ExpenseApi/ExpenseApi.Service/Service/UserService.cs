using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<User> CreateAsync(User user)
        {
            user.Id = ObjectId.GenerateNewId();
            user.Password = _passwordHasher.HashPassword(user.Password);
            return await _repository.CreateAsync(user);
        }
        public async Task<User> UpdateAsync(User user, bool isUpdatePassword = true)
        {
            if(isUpdatePassword)
                user.Password = _passwordHasher.HashPassword(user.Password);

            return await _repository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
        }

        public async Task<List<User>> FindAsync(Expression<Func<User, bool>> filterExpression)
        {
            return await _repository.FindAsync(filterExpression);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _repository.FindAsync(_ => true);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(new ObjectId(id));
        }

        
    }
}
