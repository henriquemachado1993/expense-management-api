using ExpenseApi.Domain.Entities;

namespace ExpenseApi.Domain.Interfaces
{
    public interface IExpenseService
    {
        public Task<List<Expense>> GetAllAsync();
        public Task<Expense> GetByIdAsync(string id);
        public Task<Expense> CreateAsync(Expense product);
        public Task<Expense> UpdateAsync(Expense product);
        public Task DeleteAsync(string id);
    }
}
