using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.ValueObjects;

namespace ExpenseApi.Domain.Interfaces
{
    public interface ITransactionService
    {
        public Task<ServiceResult<List<Transaction>>> GetAllAsync(Guid UserId);
        public Task<PagingResult<List<Transaction>>> GetPagedAsync(QueryCriteria<Transaction> queryCriteria);
        public Task<ServiceResult<Transaction>> GetByIdAsync(Guid userId, Guid id);
        public Task<ServiceResult<Transaction>> CreateAsync(Transaction transaction);
        public Task<ServiceResult<Transaction>> UpdateAsync(Transaction transaction);
        public Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid id);
    }
}
