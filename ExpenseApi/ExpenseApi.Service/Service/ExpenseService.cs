using MongoDB.Bson;
using ExpenseApi.Domain.Entities;
using ExpenseApi.Domain.Interfaces;
using ExpenseApi.Domain.ValueObjects;

namespace ExpenseApi.Service
{
    public class ExpenseService : IExpenseService
    {

        private readonly IBaseRepository<Expense> _repository;

        public ExpenseService(IBaseRepository<Expense> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<Expense>> CreateAsync(Expense product)
        {
            product.Id = ObjectId.GenerateNewId();
            return ServiceResult<Expense>.CreateValidResult(await _repository.CreateAsync(product));
        }

        public async Task<ServiceResult<Expense>> UpdateAsync(Expense product)
        {
            return ServiceResult<Expense>.CreateValidResult(await _repository.UpdateAsync(product));
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
        }

        public async Task<ServiceResult<List<Expense>>> GetAllAsync()
        {
            return ServiceResult<List<Expense>>.CreateValidResult(await _repository.FindAsync(_ => true));
        }

        public async Task<ServiceResult<Expense>> GetByIdAsync(string id)
        {
            return ServiceResult<Expense>.CreateValidResult(await _repository.GetByIdAsync(new ObjectId(id)));
        }
    }
}
