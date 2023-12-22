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
         Task<ServiceResult<BankAccount>> GetByIdAsync(string userId, string id);
         Task<ServiceResult<BankAccount>> CreateAsync(BankAccount bank);
         Task<ServiceResult<BankAccount>> UpdateAsync(BankAccount bank);
         Task<ServiceResult<List<BankAccount>>> GetAllAsync(string userId);
         Task<ServiceResult<bool>> DepositAsync(string id, decimal amount);
         Task<ServiceResult<bool>> WithDrawAsync(string id, decimal amount);
         Task<ServiceResult<bool>> DeleteAsync(string userId, string id);
    }
}
