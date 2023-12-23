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
            var userExists = (await _repository.FindAsync(x => x.Email == user.Email))?.FirstOrDefault();

            if (userExists != null)
                return ServiceResult<User>.CreateInvalidResult($"Já existe um usuário cadastrado com este email: {user.Email}");

            user.Id = ObjectId.GenerateNewId();
            user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.CreateAsync(user);
            entity.ClearPassword();
            return ServiceResult<User>.CreateValidResult(entity);
        }
        public async Task<ServiceResult<User>> UpdateAsync(User user, bool isUpdatePassword = true)
        {
            if (isUpdatePassword)
                user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.GetByIdAsync(user.Id);

            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            var result = await _repository.UpdateAsync(user);
            result.ClearPassword();
            return ServiceResult<User>.CreateValidResult(result);
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

        public async Task<ServiceResult<bool>> DeleteAsync(string id)
        {
            var entity = await _repository.GetByIdAsync(ObjectId.Parse(id));

            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult("Registro não encontrado.");

            await _repository.DeleteAsync(new ObjectId(id));
            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression, bool clearPassword = true)
        {
            var result = await _repository.FindAsync(filterExpression);

            if(clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult(result);
        }

        public async Task<ServiceResult<List<User>>> GetAllAsync(bool clearPassword = true)
        {
            var result = await _repository.FindAsync(_ => true);
            
            if (clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult(result);
        }

        public async Task<ServiceResult<User>> GetByIdAsync(string id, bool clearPassword = true)
        {
            var entity = await _repository.GetByIdAsync(new ObjectId(id));
            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            if (clearPassword)
                entity.ClearPassword();

            return ServiceResult<User>.CreateValidResult(entity);
        }


    }
}
