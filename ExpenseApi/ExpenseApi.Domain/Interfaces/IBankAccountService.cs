using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IBankAccountService
    {
         Task<BaseResult<BankAccount>> GetByIdAsync(Guid userId, Guid id);
         Task<BaseResult<BankAccount>> CreateAsync(BankAccount bank);
         Task<BaseResult<BankAccount>> UpdateAsync(BankAccount bank);
         Task<BaseResult<List<BankAccount>>> GetAllAsync(Guid userId);
         Task<BaseResult<bool>> DepositAsync(Guid userId, Guid id, decimal amount);
         Task<BaseResult<bool>> WithDrawAsync(Guid userId, Guid id, decimal amount);
         Task<BaseResult<bool>> DeleteAsync(Guid userId, Guid id);
    }
}
