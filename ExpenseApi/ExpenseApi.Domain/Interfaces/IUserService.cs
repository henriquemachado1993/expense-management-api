using ExpenseApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetAllAsync();
        public Task<User> GetByIdAsync(string id);
        public Task<List<User>> FindAsync(Expression<Func<User, bool>> filterExpression);
        public Task<User> CreateAsync(User user);
        public Task<User> UpdateAsync(User user, bool isUpdatePassword = true);
        public Task DeleteAsync(string id);
    }
}
