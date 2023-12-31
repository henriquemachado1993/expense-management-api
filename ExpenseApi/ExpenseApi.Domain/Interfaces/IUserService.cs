﻿using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Patterns;
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
        public Task<ServiceResult<List<User>>> GetAllAsync(bool clearPassword = true);
        public Task<ServiceResult<User>> GetByIdAsync(Guid id, bool clearPassword = true);
        public Task<ServiceResult<List<User>>> FindAsync(Expression<Func<User, bool>> filterExpression, bool clearPassword = true);
        public Task<ServiceResult<User>> CreateAsync(User user);
        public Task<ServiceResult<User>> UpdateAsync(User user);
        public Task<ServiceResult<User>> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword);
        public Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}
