using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.ValueObjects;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IBankAccountService
    {
         Task<ServiceResult<BankAccount>> GetByIdAsync(Guid userId, Guid id);
         Task<ServiceResult<BankAccount>> CreateAsync(BankAccount bank);
         Task<ServiceResult<BankAccount>> UpdateAsync(BankAccount bank);
         Task<ServiceResult<List<BankAccount>>> GetAllAsync(Guid userId);
         Task<ServiceResult<bool>> DepositAsync(Guid userId, Guid id, decimal amount);
         Task<ServiceResult<bool>> WithDrawAsync(Guid userId, Guid id, decimal amount);
         Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid id);
    }
}
