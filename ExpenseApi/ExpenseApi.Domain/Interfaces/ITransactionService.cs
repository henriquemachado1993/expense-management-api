using BeireMKit.Domain.BaseModels;
using ExpenseApi.Domain.Entities;

namespace ExpenseApi.Domain.Interfaces
{
    public interface ITransactionService
    {
        public Task<BaseResult<List<Transaction>>> GetAllAsync(Guid UserId);
        public Task<PagingResult<List<Transaction>>> GetPagedAsync(QueryCriteria<Transaction> queryCriteria);
        public Task<BaseResult<Transaction>> GetByIdAsync(Guid userId, Guid id);
        public Task<BaseResult<Transaction>> CreateAsync(Transaction transaction);
        public Task<BaseResult<Transaction>> UpdateAsync(Transaction transaction);
        public Task<BaseResult<bool>> DeleteAsync(Guid userId, Guid id);
    }
}
