using BeireMKit.Authetication.Interfaces.Jwt;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.Patterns;
using System.Linq.Expressions;

namespace ExpenseApi.Service.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _repository;
        private readonly IPasswordHasherService _passwordHasher;

        public UserService(IBaseRepository<User> repository, IPasswordHasherService passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult<User>> CreateAsync(User user)
        {
            user.Email = user.Email.Trim().ToLower();

            var userExists = (await _repository.FindAsync(x => x.Email.ToLower() == user.Email))?.FirstOrDefault();

            if (userExists != null)
                return ServiceResult<User>.CreateInvalidResult($"Já existe um usuário cadastrado com este email: {user.Email}");

            user.Id = Guid.NewGuid();
            user.Password = _passwordHasher.HashPassword(user.Password);

            var entity = await _repository.CreateAsync(user);
            entity.ClearPassword();
            return ServiceResult<User>.CreateValidResult(entity);
        }

        public async Task<ServiceResult<User>> UpdateAsync(User user)
        {
            var entity = await _repository.GetByIdAsync(user.Id);

            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            entity.Name = user.Name;
            entity.Address = user.Address;
            entity.BirthDate = user.BirthDate;

            var result = await _repository.UpdateAsync(entity);
            result.ClearPassword();
            return ServiceResult<User>.CreateValidResult(result);
        }

        public async Task<ServiceResult<User>> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
        {
            var entity = await _repository.GetByIdAsync(userId);
            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            if (!_passwordHasher.VerifyPassword(entity.Password, oldPassword))
                return ServiceResult<User>.CreateInvalidResult("A senha anterior está incorreta.");

            entity.Password = _passwordHasher.HashPassword(newPassword);

            var result = await _repository.UpdateAsync(entity);

            result.ClearPassword();

            return ServiceResult<User>.CreateValidResult(result);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return ServiceResult<bool>.CreateInvalidResult("Registro não encontrado.");

            await _repository.DeleteAsync(id);
            return ServiceResult<bool>.CreateValidResult(true);
        }

        public async Task<ServiceResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression, bool clearPassword = true)
        {
            var result = await _repository.FindAsync(filterExpression);

            if(result != null && clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult(result);
        }

        public async Task<ServiceResult<List<User>>> GetAllAsync(bool clearPassword = true)
        {
            var result = await _repository.FindAsync(_ => true);
            
            if (result != null && clearPassword)
                result.ForEach(user => { user.ClearPassword(); });

            return ServiceResult<List<User>>.CreateValidResult(result);
        }

        public async Task<ServiceResult<User>> GetByIdAsync(Guid id, bool clearPassword = true)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ServiceResult<User>.CreateInvalidResult("Registro não encontrado.");

            if (clearPassword)
                entity.ClearPassword();

            return ServiceResult<User>.CreateValidResult(entity);
        }


    }
}
