using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;
using System.Linq.Expressions;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<BaseResult<List<User>>> GetAllAsync(bool clearPassword = true);
        public Task<BaseResult<User>> GetByIdAsync(Guid id, bool clearPassword = true);
        public Task<BaseResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression, bool clearPassword = true);
        public Task<BaseResult<User>> CreateAsync(User user);
        public Task<BaseResult<User>> UpdateAsync(User user);
        public Task<BaseResult<User>> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword);
        public Task<BaseResult<bool>> DeleteAsync(Guid id);
    }
}
