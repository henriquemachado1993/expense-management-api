using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.ValueObjects;

namespace ExpenseApi.Domain.Interfaces
{
    public interface ITransactionService
    {
        public Task<ServiceResult<List<Transaction>>> GetAllAsync(string UserId);
        public Task<ServiceResult<Transaction>> GetByIdAsync(string userId, string id);
        public Task<ServiceResult<Transaction>> CreateAsync(Transaction transaction);
        public Task<ServiceResult<Transaction>> UpdateAsync(Transaction transaction);
        public Task<ServiceResult<bool>> DeleteAsync(string userId, string id);
    }
}
