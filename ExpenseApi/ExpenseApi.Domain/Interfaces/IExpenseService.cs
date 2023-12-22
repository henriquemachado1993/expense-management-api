using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.ValueObjects;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IExpenseService
    {
        public Task<ServiceResult<List<Expense>>> GetAllAsync();
        public Task<ServiceResult<Expense>> GetByIdAsync(string id);
        public Task<ServiceResult<Expense>> CreateAsync(Expense product);
        public Task<ServiceResult<Expense>> UpdateAsync(Expense product);
        public Task DeleteAsync(string id);
    }
}
