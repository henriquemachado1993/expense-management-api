using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;

namespace ExpenseApi.Domain.Interfaces
{
    public interface ITransactionCategoryService
    {
        public Task<BaseResult<List<TransactionCategory>>> GetAllAsync();
        public Task<BaseResult<TransactionCategory>> GetByIdAsync(Guid id);
        public Task<BaseResult<TransactionCategory>> CreateAsync(TransactionCategory category);
        public Task<BaseResult<TransactionCategory>> UpdateAsync(TransactionCategory category);
        public Task<BaseResult<bool>> DeleteAsync(Guid id);
    }
}
