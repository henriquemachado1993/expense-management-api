using BeireMKit.Authetication.Interfaces.Jwt;
using BeireMKit.Data.Interfaces.MongoDB;
using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace ExpenseApi.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoRepository<User> _repository;
        private readonly IPasswordHasherService _passwordHasher;

        public UserService(IMongoRepository<User> repository, IPasswordHasherService passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<BaseResult<User>> CreateAsync(User user)
        {
            user.Email = user.Email.Trim().ToLower();

            var userExists = (await _repository.FindAsync(x => x.Email.ToLower() == user.Email))?.FirstOrDefault();

            if (userExists != null)
                return BaseResult<User>.CreateInvalidResult(message: $"Já existe um usuário cadastrado com este email: {user.Email}");

            user.Id = Guid.NewGuid();
            user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.CreateAsync(user);
            entity.ClearPassword();
            return BaseResult<User>.CreateValidResult(entity);
        }

        public async Task<BaseResult<User>> UpdateAsync(User user)
        {
            var entity = await _repository.GetByIdAsync(user.Id);

            if (entity == null)
                return BaseResult<User>.CreateInvalidResult(message: "Registro não encontrado.");

            entity.Name = user.Name;
            entity.Address = user.Address;
            entity.BirthDate = user.BirthDate;

            var result = await _repository.UpdateAsync(entity);
            result.ClearPassword();
            return BaseResult<User>.CreateValidResult(result);
        }

        public async Task<BaseResult<User>> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var entity = await _repository.GetByIdAsync(userId);
            if (entity == null)
                return BaseResult<User>.CreateInvalidResult(message: "Registro não encontrado.");

            if (!_passwordHasher.VerifyPassword(entity.Password, oldPassword))
                return BaseResult<User>.CreateInvalidResult(message: "A senha anterior está incorreta.");

            entity.Password = _passwordHasher.HashPassword(newPassword);

            var result = await _repository.UpdateAsync(entity);

            result.ClearPassword();

            return BaseResult<User>.CreateValidResult(result);
        }

        public async Task<BaseResult<bool>> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return BaseResult<bool>.CreateInvalidResult(message: "Registro não encontrado.");

            await _repository.DeleteAsync(id);
            return BaseResult<bool>.CreateValidResult(true);
        }

        public async Task<BaseResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression, bool clearPassword = true)
        {
            var result = await _repository.FindAsync(filterExpression);

            if(result != null && clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return BaseResult<List<User>>.CreateValidResult(result);
        }

        public async Task<BaseResult<List<User>>> GetAllAsync(bool clearPassword = true)
        {
            var result = await _repository.FindAsync(_ => true);
            
            if (result != null && clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return BaseResult<List<User>>.CreateValidResult(result);
        }

        public async Task<BaseResult<User>> GetByIdAsync(Guid id, bool clearPassword = true)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return BaseResult<User>.CreateInvalidResult(message: "Registro não encontrado.");

            if (clearPassword)
                entity.ClearPassword();

            return BaseResult<User>.CreateValidResult(entity);
        }


    }
}
