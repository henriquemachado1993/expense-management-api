using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;

namespace ExpenseApi.Service
{
    public class ExpenseService : IExpenseService
    {

        private readonly IBaseRepository<Expense> _repository;

        public ExpenseService(IBaseRepository<Expense> repository)
        {
            _repository = repository;
        }

        public async Task<Expense> CreateAsync(Expense product)
        {
            product.Id = ObjectId.GenerateNewId();
            return await _repository.CreateAsync(product);
        }

        public async Task<Expense> UpdateAsync(Expense product)
        {
            return await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
        }

        public async Task<List<Expense>> GetAllAsync()
        {
            return await _repository.FindAsync(_ => true);
        }

        public async Task<Expense> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(new ObjectId(id));
        }
    }
}
